using BepInEx;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Vectorio.Entities;

[BepInPlugin("your.vectorio.blueprintcopytool", "BlueprintCopyTool", "1.4.0")]
public class BlueprintCopyToolMain : BaseUnityPlugin
{
    void Awake()
    {
        Logger.LogInfo("BlueprintCopyToolMain Awake: attaching BlueprintCopyRunner.");
        var go = new GameObject("BlueprintCopyRunner");
        var runner = go.AddComponent<BlueprintCopyRunner>();
        runner.Logger = this.Logger;
        DontDestroyOnLoad(go);
    }
}

public class BlueprintCopyRunner : MonoBehaviour
{
    public BepInEx.Logging.ManualLogSource Logger;

    const int TILE_SIZE = 5;
    bool selectionMode = false;
    Vector2Int regionStart = new Vector2Int(0, 0);
    Vector2Int regionSize = new Vector2Int(3, 3); // default size
    RectInt selectedRegion;
    List<(string entityId, Vector2Int cell)> selectedEntities = new List<(string, Vector2Int)>();

    void Update()
    {
        // Toggle selection mode with B
        if (Input.GetKeyDown(KeyCode.B))
        {
            selectionMode = !selectionMode;
            if (selectionMode)
            {
                Vector3 camMid = Camera.main.transform.position;
                Vector3Int cellMid = new Vector3Int(Mathf.RoundToInt(camMid.x / TILE_SIZE), Mathf.RoundToInt(camMid.y / TILE_SIZE), 0);
                regionStart = new Vector2Int(cellMid.x, cellMid.y);
                regionSize = new Vector2Int(3, 3); // Start with a 3x3 box
                selectedRegion = new RectInt(regionStart.x, regionStart.y, regionSize.x, regionSize.y);
                selectedEntities = FindEntitiesInRegion(selectedRegion);
                Logger.LogInfo("[BlueprintCopyTool] Selection mode enabled at " + regionStart);
            }
            else
            {
                Logger.LogInfo("[BlueprintCopyTool] Selection mode disabled");
            }
        }

        if (!selectionMode) return;
        bool moved = false;

        // -- Movement of the whole region with no modifiers --
        if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift) &&
            !Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt))
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { regionStart.x -= 1; moved = true; }
            if (Input.GetKeyDown(KeyCode.RightArrow)) { regionStart.x += 1; moved = true; }
            if (Input.GetKeyDown(KeyCode.UpArrow)) { regionStart.y += 1; moved = true; }
            if (Input.GetKeyDown(KeyCode.DownArrow)) { regionStart.y -= 1; moved = true; }
        }

        // -- Shift (expand outwards): add cells at the boundaries --
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) &&
            !Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt))
        {
            // Expand lower boundary (grow down)
            if (Input.GetKeyDown(KeyCode.UpArrow)) { regionSize.y = Mathf.Max(1, regionSize.y + 1); moved = true; }
            // Expand upper boundary (grow up) = move start upward & increase height
            if (Input.GetKeyDown(KeyCode.DownArrow)) { regionStart.y -= 1; regionSize.y = Mathf.Max(1, regionSize.y + 1); moved = true; }
            // Expand left boundary (grow left): move start left and make wider
            if (Input.GetKeyDown(KeyCode.RightArrow)) { regionStart.x -= 1; regionSize.x = Mathf.Max(1, regionSize.x + 1); moved = true; }
            // Expand right boundary (grow right)
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { regionSize.x = Mathf.Max(1, regionSize.x + 1); moved = true; }
        }

        // -- Alt+Shift (contract/trim boundaries -- remove cells at edges) --
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) &&
            (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)))
        {
            // SHRINK FROM TOP (raise lower boundary): move start down, decrease height
            if (Input.GetKeyDown(KeyCode.DownArrow) && regionSize.y > 1)
            {
                regionStart.y += 1; regionSize.y -= 1; moved = true;
            }
            // SHRINK FROM BOTTOM (raise upper boundary): just decrease height
            if (Input.GetKeyDown(KeyCode.UpArrow) && regionSize.y > 1)
            {
                regionSize.y -= 1; moved = true;
            }
            // SHRINK FROM LEFT (raise left boundary): move start right, decrease width
            if (Input.GetKeyDown(KeyCode.RightArrow) && regionSize.x > 1)
            {
                regionStart.x += 1; regionSize.x -= 1; moved = true;
            }
            // SHRINK FROM RIGHT (raise right boundary): decrease width
            if (Input.GetKeyDown(KeyCode.LeftArrow) && regionSize.x > 1)
            {
                regionSize.x -= 1; moved = true;
            }
        }

        // Clamp to minimum size 1x1 always
        regionSize.x = Mathf.Max(1, regionSize.x); regionSize.y = Mathf.Max(1, regionSize.y);

        if (moved)
        {
            selectedRegion = new RectInt(regionStart.x, regionStart.y, regionSize.x, regionSize.y);
            selectedEntities = FindEntitiesInRegion(selectedRegion);
        }

        // Copy with Ctrl+C
        if (
            (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            && Input.GetKeyDown(KeyCode.C)
            && selectedEntities.Count > 0)
        {
            ExportSelection(selectedEntities, selectedRegion);
        }
    }

    List<(string entityId, Vector2Int cell)> FindEntitiesInRegion(RectInt region)
    {
        var found = new List<(string, Vector2Int)>();
        var tileGrid = Singleton<TileGrid>.Instance;
        if (tileGrid == null) return found;

        for (int x = region.xMin; x < region.xMax; x++)
            for (int y = region.yMin; y < region.yMax; y++)
            {
                Vector2Int cell = new Vector2Int(x, y);
                var cellData = tileGrid.GetCell(cell, verifyCoords: true);
                string id = "none";
                if (cellData != null && cellData.GetOccupier != null)
                {
                    var building = cellData.GetOccupier as Building;
                    if (building != null)
                        id = building.EntityID;
                }
                found.Add((id, cell));
            }
        return found;
    }

    void ExportSelection(List<(string id, Vector2Int cell)> entities, RectInt region)
    {
        int w = region.width, h = region.height;
        string[] rows = new string[h];
        for (int y = 0; y < h; y++)
        {
            List<string> row = new List<string>();
            int gridY = region.yMax - 1 - y; // Top row first!
            for (int x = 0; x < w; x++)
            {
                int gridX = region.xMin + x;
                var ent = entities.Find(e => e.cell.x == gridX && e.cell.y == gridY);
                row.Add(ent.id ?? "none");
            }
            rows[y] = string.Join(", ", row) + ";";
        }
        string result = $"{w}x{h};\n{string.Join("\n", rows)}";
        var path = Path.Combine(Paths.BepInExRootPath, "in.txt");
        try
        {
            File.WriteAllText(path, result);
            Logger.LogInfo($"[BlueprintCopyTool] Exported {entities.Count} entities as a {w}x{h} grid to in.txt!");
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            GUIUtility.systemCopyBuffer = result;
#endif
        }
        catch (System.Exception ex)
        {
            Logger.LogError("[BlueprintCopyTool] Failed to export: " + ex);
        }
    }

    void OnGUI()
    {
        if (selectionMode)
        {
            Vector3 p1 = ToWorldCorner(new Vector3Int(selectedRegion.xMin, selectedRegion.yMin, 0));
            Vector3 p2 = ToWorldCorner(new Vector3Int(selectedRegion.xMax, selectedRegion.yMax, 0));

            Vector3 scr1 = Camera.main.WorldToScreenPoint(p1);
            Vector3 scr2 = Camera.main.WorldToScreenPoint(p2);

            float x = Mathf.Min(scr1.x, scr2.x);
            float y = Screen.height - Mathf.Max(scr1.y, scr2.y);
            float w = Mathf.Abs(scr1.x - scr2.x);
            float h = Mathf.Abs(scr1.y - scr2.y);

            Color prev = GUI.color;
            GUI.color = new Color(0f, 1f, 1f, 0.3f);
            GUI.DrawTexture(new Rect(x, y, w, h), Texture2D.whiteTexture);
            GUI.color = prev;

            GUI.Label(new Rect(6, 6, 1000, 52),
                "[BlueprintCopyTool] Selection mode: " +
                "B=exit, arrows=move region, SHIFT+arrows=expand (left/right/up/down), ALT+SHIFT+arrows=raise boundary (contract), Ctrl+C=copy.\n" +
                $"Region: {selectedRegion.xMin},{selectedRegion.yMin}, size {selectedRegion.width}x{selectedRegion.height}");
        }
    }

    Vector3 ToWorldCorner(Vector3Int cell)
    {
        Vector3 origin = Singleton<TileGrid>.Instance.primaryTilemap.CellToWorld(Vector3Int.zero);
        return new Vector3(origin.x + cell.x * TILE_SIZE, origin.y + cell.y * TILE_SIZE, 0f);
    }
}
