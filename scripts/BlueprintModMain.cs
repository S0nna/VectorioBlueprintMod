using BepInEx;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Vectorio.Entities;

[BepInPlugin("vectorio.blueprintimporter", "StorageGridBlueprintImporter", "1.2.0")]
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

        // Place top-left of blueprint under mouse cursor
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        int startX = Mathf.RoundToInt(mouseWorld.x / TILE_SIZE) * TILE_SIZE;
        int startY = Mathf.RoundToInt(mouseWorld.y / TILE_SIZE) * TILE_SIZE;

        Logger.LogInfo($"[StorageGridBlueprintImporter] Placing {width}x{height} blueprint at ({startX},{startY}) tile origin.");

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                string id = entityGrid[y][x].Trim();
                if (id != "" && id.ToLower() != "none")
                {
                    Vector3Int pos = new Vector3Int(startX + x * TILE_SIZE, startY - y * TILE_SIZE, 0);
                    QueueEntityBuild(id, pos);
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
                IsBlueprint = true, // Place as blueprint
                ModelID = "default",
                FactionID = Singleton<FactionManager>.Instance.PlayerFactionID
            };

            var entityManager = Singleton<EntityManager>.Instance;
            entityManager.QueueCreationEvent(creationData);

            // Ensure the blueprint and resulting built entity are editable/removable
            StartCoroutine(MakeEditableNextFrame(new Vector2Int(position.x, position.y), entityData.ID));
        }
        catch (System.Exception ex)
        {
            Logger.LogError($"[StorageGridBlueprintImporter] Exception for '{entityName}' at {position}: {ex}");
        }
    }

    IEnumerator MakeEditableNextFrame(Vector2Int gridPos, string entityTypeId)
    {
        yield return null; // Wait one frame for entity to spawn

        var entityMgr = Singleton<EntityManager>.Instance;
        if (entityMgr == null) yield break;

        bool found = false;
        foreach (var ent in entityMgr.Entities.Values)
        {
            Vector3 pos = ent.transform.position;
            if (Mathf.RoundToInt(pos.x) == gridPos.x && Mathf.RoundToInt(pos.y) == gridPos.y && ent.ID == entityTypeId)
            {
                ent.Set_EFlag_IsEditable(true);
                ent.Set_EFlag_IsDead(false);
                ent.IsSaveable = true;
                Logger.LogInfo($"[BlueprintImporter] Set entity/blueprint '{ent.ID}' at {gridPos} (blueprint? {ent.Has_EFlag_IsBlueprint}) to editable.");
                found = true;
                break;
            }
        }
        if (!found)
        {
            Logger.LogWarning($"[BlueprintImporter] No entity found at {gridPos} with ID '{entityTypeId}' to make editable.");
        }

        // --- New: also make built entity editable after blueprint is completed --- 
        StartCoroutine(MakeBuiltEditableWhenConstructed(gridPos, entityTypeId));
    }

    IEnumerator MakeBuiltEditableWhenConstructed(Vector2Int gridPos, string entityTypeId)
    {
        // Try for 10s, checking every 0.5s, in case construction takes time.
        for (float timer = 0; timer < 10f; timer += 0.5f)
        {
            yield return new WaitForSeconds(0.5f);
            var entityMgr = Singleton<EntityManager>.Instance;
            if (entityMgr == null) yield break;

            foreach (var ent in entityMgr.Entities.Values)
            {
                Vector3 pos = ent.transform.position;
                if (Mathf.RoundToInt(pos.x) == gridPos.x && Mathf.RoundToInt(pos.y) == gridPos.y &&
                    ent.ID == entityTypeId && !ent.Has_EFlag_IsBlueprint)
                {
                    ent.Set_EFlag_IsEditable(true);
                    ent.Set_EFlag_IsDead(false);
                    ent.IsSaveable = true;
                    Logger.LogInfo($"[BlueprintImporter] Set built entity '{ent.ID}' at {gridPos} to editable after completion.");
                    yield break;
                }
            }
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
