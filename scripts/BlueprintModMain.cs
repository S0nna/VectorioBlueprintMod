using BepInEx;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System.Collections.Generic;
using Vectorio.Entities;

[BepInPlugin("vectorio.blueprintimporter", "StorageGridBlueprintImporter", "1.5.0")]
public class StorageGridBlueprintImporterMain : BaseUnityPlugin
{
    void Awake()
    {
        Logger.LogInfo("StorageGridBlueprintImporterMain Awake: attaching runner.");
        var runnerGO = new GameObject("StorageGridBlueprintRunner");
        var runner = runnerGO.AddComponent<StorageGridBlueprintRunner>();
        runner.Logger = this.Logger;
        DontDestroyOnLoad(runnerGO);
    }
}

public class StorageGridBlueprintRunner : MonoBehaviour
{
    public BepInEx.Logging.ManualLogSource Logger;
    const int TILE_SIZE = 5;
    const string INPUT_FILE = "in.txt";
    string consoleInput = "";
    bool showConsole = false;
    Vector2 scroll = Vector2.zero;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            showConsole = !showConsole;
            if (showConsole) consoleInput = "";
        }
        if (Input.GetKeyDown(KeyCode.N) && !showConsole)
        {
            PlaceBlueprintFromFile();
        }
    }

    void PlaceBlueprintFromFile()
    {
        if (!IsReady())
        {
            Logger.LogWarning("[StorageGridBlueprintImporter] Not ready or dependencies missing!");
            return;
        }

        string path = Path.Combine(Paths.BepInExRootPath, INPUT_FILE);
        if (!File.Exists(path))
        {
            Logger.LogError($"[StorageGridBlueprintImporter] Input file not found: {path}");
            return;
        }

        List<List<string>> entityGrid;
        int width, height;
        if (!ParseBlueprintFile(path, out entityGrid, out width, out height))
        {
            Logger.LogError("[StorageGridBlueprintImporter] Failed to parse input file (syntax?).");
            return;
        }

        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        int startX = Mathf.RoundToInt(mouseWorld.x / TILE_SIZE) * TILE_SIZE;
        int startY = Mathf.RoundToInt(mouseWorld.y / TILE_SIZE) * TILE_SIZE;

        Logger.LogInfo($"[StorageGridBlueprintImporter] Placing {width}x{height} blueprint at ({startX},{startY}) tile origin.");

        // Robust placement for multi-cell and 1x1 entities, with anchor correction
        bool[,] placed = new bool[height, width];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (placed[y, x]) continue;
                string id = entityGrid[y][x].Trim();
                if (id == "" || id.ToLower() == "none") continue;

                int entityWidth = 1, entityHeight = 1;
                var entityData = Library.RequestData<EntityData>(id);
                if (entityData != null)
                {
                    var buildingData = entityData.GetComponent<BuildingData>();
                    if (buildingData != null)
                    {
                        entityWidth = buildingData.Width;
                        entityHeight = buildingData.Height;
                    }
                }

                // --- Correct placement for anchors (so big entities snap perfectly) ---
                float offsetX = ((float)entityWidth / 2f - 0.5f) * TILE_SIZE;
                float offsetY = -((float)entityHeight / 2f - 0.5f) * TILE_SIZE;

                Vector3Int pos = new Vector3Int(
                    startX + x * TILE_SIZE + Mathf.RoundToInt(offsetX),
                    startY - y * TILE_SIZE + Mathf.RoundToInt(offsetY),
                    0
                );
                QueueEntityBuild(id, pos);

                // Mark all covered cells by this entity as "placed"
                for (int dy = 0; dy < entityHeight; dy++)
                    for (int dx = 0; dx < entityWidth; dx++)
                    {
                        int tx = x + dx, ty = y + dy;
                        if (ty < height && tx < width)
                            placed[ty, tx] = true;
                    }
            }
        }
    }

    void OnGUI()
    {
        if (!showConsole) return;
        int w = 500, h = 200;
        Rect winRect = new Rect(Screen.width / 2 - w / 2, Screen.height / 2 - h / 2, w, h);

        GUI.WindowFunction func = id =>
        {
            GUILayout.Label("Paste your blueprint here (eg: 2x2; vec_storage,vec_storage; vec_storage,vec_storage;)");
            scroll = GUILayout.BeginScrollView(scroll, false, false);
            GUI.SetNextControlName("PasteBox");
            consoleInput = GUILayout.TextArea(consoleInput, GUILayout.ExpandHeight(true), GUILayout.MinHeight(100));
            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Apply & Save (use with N key)"))
            {
                string path = Path.Combine(Paths.BepInExRootPath, INPUT_FILE);
                try
                {
                    File.WriteAllText(path, consoleInput.Replace("\\n", "\n").Replace("\r\n", "\n").Replace("\r", "\n"));
                    Logger.LogInfo("[StorageGridBlueprintImporter] Blueprint updated via in-game paste!");
                }
                catch (System.Exception ex)
                {
                    Logger.LogError("[StorageGridBlueprintImporter] Failed to write file: " + ex);
                }
                showConsole = false;
            }

            if (GUILayout.Button("Cancel") || Event.current.keyCode == KeyCode.Escape)
            {
                showConsole = false;
            }
            GUILayout.EndHorizontal();

            GUI.FocusControl("PasteBox");
        };
        GUILayout.Window(17777744, winRect, func, "Paste Blueprint (In-Game)");
    }

    bool IsReady()
    {
        return Camera.main != null &&
               Singleton<EntityManager>.Instance != null;
    }

    void QueueEntityBuild(string entityName, Vector3Int position)
    {
        try
        {
            var entityData = Library.RequestData<EntityData>(entityName);
            if (entityData == null)
            {
                Logger.LogWarning($"[StorageGridBlueprintImporter] Entity '{entityName}' not found! Skipping.");
                return;
            }

            var creationData = new EntityCreationData
            {
                EntityID = entityData.ID,
                PosX = position.x,
                PosY = position.y,
                SyncType = SyncType.ServerInitiated,
                IsBlueprint = true,
                ModelID = "default",
                FactionID = Singleton<FactionManager>.Instance.PlayerFactionID,
                EntityFlags = EntityFlags.IsEditable  // Key: always deletable!
            };

            var entityManager = Singleton<EntityManager>.Instance;
            entityManager.QueueCreationEvent(creationData);
        }
        catch (System.Exception ex)
        {
            Logger.LogError($"[StorageGridBlueprintImporter] Exception for '{entityName}' at {position}: {ex}");
        }
    }

    // Parses the text file into a grid of entity ID strings
    bool ParseBlueprintFile(string path, out List<List<string>> grid, out int width, out int height)
    {
        grid = new List<List<string>>();
        width = height = 0;
        try
        {
            var allLines = File.ReadAllLines(path);
            if (allLines.Length < 2)
                return false;

            var header = allLines[0].Trim().Replace(";", "");
            var dims = header.Split('x');
            if (dims.Length != 2)
                return false;

            width = int.Parse(dims[0]);
            height = int.Parse(dims[1]);

            for (int i = 1; i <= height; i++)
            {
                var row = allLines[i].Replace(";", "").Trim().Split(',');
                List<string> ids = new List<string>();
                foreach (var cell in row)
                    ids.Add(cell.Trim());
                grid.Add(ids);
            }

            if (grid.Count != height)
                return false;
            foreach (var line in grid)
                if (line.Count != width)
                    return false;

            return true;
        }
        catch (System.Exception ex)
        {
            Logger.LogError($"[StorageGridBlueprintImporter] Parse error: {ex}");
            return false;
        }
    }
}
