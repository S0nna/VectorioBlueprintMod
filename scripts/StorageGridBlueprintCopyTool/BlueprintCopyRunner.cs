using BepInEx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Vectorio.Entities;

public class BlueprintCopyRunner : MonoBehaviour
{
    public BepInEx.Logging.ManualLogSource Logger;
    const int TILE_SIZE = 5;
    bool selectionMode = false;
    Vector2Int regionStart = Vector2Int.zero;
    Vector2Int regionSize = new(3, 3);
    RectInt selectedRegion;
    List<(string entityId, Vector2Int cell)> selectedEntities = new();

    struct DroneMeta
    {
        public Vector2Int DronePos;
        public List<Vector2Int> PickupPositions;
        public List<Vector2Int> DropoffPositions;
        public string ResourceID;
    }

    void Update()
    {
        // Start selection
        if (Input.GetKeyDown(KeyCode.B))
        {
            selectionMode = !selectionMode;
            if (selectionMode)
            {
                Vector3 mouse = Camera.main.ScreenToWorldPoint(new Vector3(
                    Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z)));
                regionStart = new Vector2Int(Mathf.FloorToInt(mouse.x / TILE_SIZE), Mathf.FloorToInt(mouse.y / TILE_SIZE));
                regionSize = new(3, 3);
                selectedRegion = new RectInt(regionStart.x, regionStart.y, regionSize.x, regionSize.y);
                selectedEntities = FindEntitiesInRegion(selectedRegion);
                Logger.LogInfo("[BlueprintCopyTool] Selection started at " + regionStart);
            }
            else Logger.LogInfo("[BlueprintCopyTool] Selection ended");
        }

        if (!selectionMode) return;

        bool changed = false;
        
        // Check for Alt+Shift+Arrow first (highest priority)
        bool altPressed = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
        bool shiftPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        
        if (altPressed && shiftPressed)
        {
            // Shrink with Alt+Shift
            if (Input.GetKeyDown(KeyCode.RightArrow) && regionSize.x > 1) 
            { 
                regionSize.x -= 1; 
                changed = true; 
                Logger.LogInfo($"[BlueprintCopyTool] Shrinking right, new size: {regionSize.x}x{regionSize.y}");
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && regionSize.x > 1) 
            { 
                regionStart.x += 1; 
                regionSize.x -= 1; 
                changed = true; 
                Logger.LogInfo($"[BlueprintCopyTool] Shrinking left, new size: {regionSize.x}x{regionSize.y}");
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && regionSize.y > 1) 
            { 
                regionSize.y -= 1; 
                changed = true; 
                Logger.LogInfo($"[BlueprintCopyTool] Shrinking up, new size: {regionSize.x}x{regionSize.y}");
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && regionSize.y > 1) 
            { 
                regionStart.y += 1; 
                regionSize.y -= 1; 
                changed = true; 
                Logger.LogInfo($"[BlueprintCopyTool] Shrinking down, new size: {regionSize.x}x{regionSize.y}");
            }
        }
        // Move selection (no modifiers)
        else if (!shiftPressed && !altPressed)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { regionStart.x -= 1; changed = true; }
            if (Input.GetKeyDown(KeyCode.RightArrow)) { regionStart.x += 1; changed = true; }
            if (Input.GetKeyDown(KeyCode.UpArrow)) { regionStart.y += 1; changed = true; }
            if (Input.GetKeyDown(KeyCode.DownArrow)) { regionStart.y -= 1; changed = true; }
        }
        // Resize with Shift only
        else if (shiftPressed && !altPressed)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) { regionSize.x += 1; changed = true; }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { regionStart.x -= 1; regionSize.x += 1; changed = true; }
            if (Input.GetKeyDown(KeyCode.UpArrow)) { regionSize.y += 1; changed = true; }
            if (Input.GetKeyDown(KeyCode.DownArrow)) { regionStart.y -= 1; regionSize.y += 1; changed = true; }
        }
        regionSize.x = Mathf.Max(1, regionSize.x);
        regionSize.y = Mathf.Max(1, regionSize.y);

        if (changed)
        {
            selectedRegion = new RectInt(regionStart.x, regionStart.y, regionSize.x, regionSize.y);
            selectedEntities = FindEntitiesInRegion(selectedRegion);
        }

        // Export (Ctrl+C)
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) &&
            Input.GetKeyDown(KeyCode.C) && selectedEntities.Count > 0)
        {
            ExportSelection(selectedEntities, selectedRegion);
        }
    }

    List<(string entityId, Vector2Int cell)> FindEntitiesInRegion(RectInt region)
    {
        var found = new List<(string, Vector2Int)>();
        var grid = Singleton<TileGrid>.Instance;
        if (grid == null) return found;
        for (int x = region.xMin; x < region.xMax; x++)
            for (int y = region.yMin; y < region.yMax; y++)
            {
                Vector2Int cell = new(x, y);
                var cellData = grid.GetCell(cell, true);
                string id = "none";
                if (cellData?.GetOccupier is Building b)
                    id = b.EntityID;
                found.Add((id, cell));
            }
        return found;
    }

    Entity GetEntityAtCell(Vector2Int cell)
    {
        var mgr = Singleton<EntityManager>.Instance;
        if (mgr == null) return null;
        foreach (var entity in mgr.Entities.Values)
        {
            if (!entity) continue;
            Vector3 pos = entity.transform.position;
            int gridX = Mathf.RoundToInt(pos.x / TILE_SIZE);
            int gridY = Mathf.RoundToInt(pos.y / TILE_SIZE);
            if (gridX == cell.x && gridY == cell.y) return entity;
        }
        return null;
    }

    void ExportSelection(List<(string id, Vector2Int cell)> entities, RectInt region)
    {
        int w = region.width, h = region.height;
        string[] rows = new string[h];
        var droneMetadata = new List<DroneMeta>();

        for (int y = 0; y < h; y++)
        {
            List<string> row = new();
            int gridY = region.yMax - 1 - y;
            for (int x = 0; x < w; x++)
            {
                int gridX = region.xMin + x;
                var ent = entities.Find(e => e.cell.x == gridX && e.cell.y == gridY);
                string id = ent.id ?? "none";
                row.Add(id);

                Entity entity = GetEntityAtCell(ent.cell);
                if (entity == null) continue;
                var port = entity.Get_EComponent<Port>(false);
                if (port == null) continue;
                var drone = port.GetDrone as CargoDrone;
                if (drone == null) continue;
                var pickups = drone.PickupCoverage?.targets;
                var dropoffs = drone.DropoffCoverage?.targets;

                List<Vector2Int> pickupPositions = new();
                List<Vector2Int> dropoffPositions = new();

                if (pickups != null)
                    foreach (var t in pickups)
                    {
                        Vector3 pos = t.transform.position;
                        int relX = Mathf.RoundToInt(pos.x / TILE_SIZE) - region.xMin;
                        int relY = Mathf.RoundToInt(pos.y / TILE_SIZE) - region.yMin;
                        pickupPositions.Add(new Vector2Int(relX, relY));
                    }
                if (dropoffs != null)
                    foreach (var t in dropoffs)
                    {
                        Vector3 pos = t.transform.position;
                        int relX = Mathf.RoundToInt(pos.x / TILE_SIZE) - region.xMin;
                        int relY = Mathf.RoundToInt(pos.y / TILE_SIZE) - region.yMin;
                        dropoffPositions.Add(new Vector2Int(relX, relY));
                    }

                string resourceID = drone.Filter?.ID;

                if (pickupPositions.Count > 0 || dropoffPositions.Count > 0 || !string.IsNullOrEmpty(resourceID))
                    droneMetadata.Add(new DroneMeta
                    {
                        DronePos = new Vector2Int(x, y),
                        PickupPositions = pickupPositions,
                        DropoffPositions = dropoffPositions,
                        ResourceID = resourceID
                    });
            }
            rows[y] = string.Join(", ", row) + ";";
        }

        string result = $"{w}x{h};\n{string.Join("\n", rows)}";

        if (droneMetadata.Count > 0)
        {
            result += "\n[DroneDestinations]\n";
            foreach (var meta in droneMetadata)
            {
                var pickupsStr = string.Join(",", meta.PickupPositions.ConvertAll(p => $"({p.x},{p.y})"));
                var dropoffsStr = string.Join(",", meta.DropoffPositions.ConvertAll(p => $"({p.x},{p.y})"));

                List<string> parts = new();
                if (meta.PickupPositions.Count > 0) parts.Add($"pickup=[{pickupsStr}]");
                if (meta.DropoffPositions.Count > 0) parts.Add($"dropoff=[{dropoffsStr}]");
                if (!string.IsNullOrEmpty(meta.ResourceID)) parts.Add($"resource={meta.ResourceID}");

                result += $"({meta.DronePos.x},{meta.DronePos.y}): {string.Join(", ", parts)}\n";
            }
        }
        try
        {
            File.WriteAllText(Path.Combine(Paths.BepInExRootPath, "in.txt"), result);
            Logger.LogInfo($"[BlueprintCopyTool] Exported region {w}x{h}, {droneMetadata.Count} drone(s) to in.txt");
        }
        catch (Exception ex)
        {
            Logger.LogError($"[BlueprintCopyTool] Failed to export: {ex}");
        }
    }

    void OnGUI()
    {
        if (!selectionMode) return;
        var grid = Singleton<TileGrid>.Instance;
        if (Camera.main == null || grid == null) return;

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
            "[BlueprintCopyTool] Selection mode: B=exit, arrows=move, shift+arrows=expand,\nalt+shift+arrows=shrink, Ctrl+C=copy\n" +
            $"Region: {selectedRegion.xMin},{selectedRegion.yMin}, size {selectedRegion.width}x{selectedRegion.height}");
    }

    Vector3 ToWorldCorner(Vector3Int cell)
    {
        Vector3 o = Singleton<TileGrid>.Instance.primaryTilemap.CellToWorld(Vector3Int.zero);
        return new Vector3(o.x + cell.x * TILE_SIZE, o.y + cell.y * TILE_SIZE, 0f);
    }
}
