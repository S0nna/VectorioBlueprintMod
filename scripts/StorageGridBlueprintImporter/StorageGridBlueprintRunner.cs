using BepInEx;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Vectorio.Entities;
using Vectorio.Utilities;

public class StorageGridBlueprintRunner : MonoBehaviour
{
    public BepInEx.Logging.ManualLogSource Logger;
    const int TILE_SIZE = 5;
    const string INPUT_FILE = "in.txt";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Logger.LogInfo("[BlueprintImporter] N key pressed — placing blueprint.");
            StartCoroutine(PlaceBlueprintFromFile());
        }
    }

    IEnumerator PlaceBlueprintFromFile()
    {
        string path = Path.Combine(Paths.BepInExRootPath, INPUT_FILE);
        if (!File.Exists(path))
        {
            Logger.LogError("[BlueprintImporter] in.txt not found at: " + path);
            yield break;
        }

        if (!TryParseBlueprint(path, out var grid, out var width, out var height, out var droneMeta, out var allLines))
        {
            Logger.LogError("[BlueprintImporter] Failed to parse blueprint.");
            yield break;
        }

        Vector3 mouse = Input.mousePosition;
        mouse.z = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 world = Camera.main.ScreenToWorldPoint(mouse);

        int baseX = Mathf.RoundToInt(world.x / TILE_SIZE) * TILE_SIZE;
        int baseY = Mathf.RoundToInt(world.y / TILE_SIZE) * TILE_SIZE;

        // Origin of blueprint grid (top-left tile) in world tile coordinates
        int originX = baseX / TILE_SIZE;
        int originY = baseY / TILE_SIZE;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                string id = grid[y][x];
                if (string.IsNullOrEmpty(id) || id == "none")
                    continue;

                var data = Library.RequestData<EntityData>(id);
                if (data == null) continue;

                var buildingData = data.GetComponent<BuildingData>();
                if (buildingData == null) continue;

                int entWidth = buildingData.Width;
                int entHeight = buildingData.Height;

                float offsetX = (entWidth - 1) * 0.5f * TILE_SIZE;
                float offsetY = (entHeight - 1) * 0.5f * TILE_SIZE;

                float worldX = baseX + x * TILE_SIZE;
                float worldY = baseY - y * TILE_SIZE;

                Vector3 position = new Vector3(worldX + offsetX, worldY - offsetY, 0f);

                QueueEntityBuildReturn(id, position);

                // Attach drone route metadata if needed
                Vector2Int relPos = new(x, y);
                if (droneMeta.TryGetValue(relPos, out var meta))
                {
                    Vector2Int worldTile = new(originX + x, originY - y);
                    StartCoroutine(ApplyDroneRouteWhenReady(worldTile, meta, originX, originY, x, y, height));
                }
            }
        }


        Logger.LogInfo("[BlueprintImporter] Blueprint placement complete.");
    }

    IEnumerator ApplyDroneRouteWhenReady(
    Vector2Int portTile,
    DroneRouteBlueprintData meta,
    int originX,
    int originY,
    int portBlueprintX,
    int portBlueprintY,
    int blueprintHeight)

    {
        Entity targetPortEntity = null;
        Port port = null;
        int frameCount = 0;

        // Wait for the port to spawn at the tile
        while (true)
        {
            foreach (var entity in Singleton<EntityManager>.Instance.Entities.Values)
            {
                Vector2Int entityTile = Utilities.ConvertWorldPositionToCell(entity.transform.position);
                if (entityTile == portTile)
                {
                    var possiblePort = entity.Get_EComponent<Port>(false);
                    if (possiblePort != null)
                    {
                        targetPortEntity = entity;
                        port = possiblePort;
                        break;
                    }
                }
            }

            if (targetPortEntity != null && port != null)
                break;

            if (frameCount % 300 == 0)
                Logger.LogInfo($"[BlueprintImporter] Waiting for Port entity at {portTile}... after {frameCount} frames.");
            yield return null;
            frameCount++;
        }

        // Wait for drone to be attached to that port
        frameCount = 0;
        while (port.GetDrone == null)
        {
            if (frameCount % 300 == 0)
                Logger.LogInfo($"[BlueprintImporter] Waiting for CargoDrone at {portTile}... after {frameCount} frames.");
            yield return null;
            frameCount++;
        }

        var drone = port.GetDrone as CargoDrone;
        if (drone == null)
        {
            Logger.LogWarning($"[BlueprintImporter] No CargoDrone found at {portTile} after waiting.");
            yield break;
        }

        Logger.LogInfo($"[BlueprintImporter] ✅ CargoDrone found at {portTile}");

        // Apply resource filter if provided
        if (!string.IsNullOrEmpty(meta.resourceID))
        {
            var filter = Library.RequestData<ResourceData>(meta.resourceID);
            if (filter != null)
            {
                drone.SyncFilter(filter);
                Logger.LogInfo($"[BlueprintImporter] Applied filter '{meta.resourceID}' at {portTile}");
            }
        }

        if (meta.pickup != null && meta.pickup.Count > 0 && meta.dropoff != null && meta.dropoff.Count > 0)
        {
            Logger.LogInfo($"[BlueprintImporter] Port blueprint grid position: ({portBlueprintX}, {portBlueprintY})");
            Logger.LogInfo($"[BlueprintImporter] Port world tile: {portTile}");

            foreach (var p in meta.pickup)
            {
                var w = new Vector2Int(originX + p.x, originY - p.y);
                var offset = w - portTile;
                Logger.LogInfo($"  ➜ Pickup blueprint {p} → world {w}, offset from drone: {offset}");
            }

            foreach (var d in meta.dropoff)
            {
                var w = new Vector2Int(originX + d.x, originY - d.y);
                var offset = w - portTile;
                Logger.LogInfo($"  ➜ Dropoff blueprint {d} → world {w}, offset from drone: {offset}");
            }

            var pickup = BuildCoverageWorldAbs(meta.pickup, originX, originY, blueprintHeight);
            var dropoff = BuildCoverageWorldAbs(meta.dropoff, originX, originY, blueprintHeight);

            if (pickup != null && dropoff != null)
            {
                Logger.LogInfo($"[BlueprintImporter] Applying drone route at {portTile}");
                Logger.LogInfo($"  ▸ Pickup Coverage:  ({pickup.startX},{pickup.startY}) → ({pickup.endX},{pickup.endY})");
                Logger.LogInfo($"  ▸ Dropoff Coverage: ({dropoff.startX},{dropoff.startY}) → ({dropoff.endX},{dropoff.endY})");

                drone.CreateCoverage(pickup, dropoff);
            }
            else
            {
                Logger.LogWarning($"[BlueprintImporter] Coverage failed to build at {portTile} (pickup/dropoff was null).");
            }
        }
        else
        {
            Logger.LogWarning($"[BlueprintImporter] Missing pickup or dropoff tiles in metadata for drone at {portTile}.");
        }

        Logger.LogInfo($"[BlueprintImporter] ✅ Route assignment complete for drone at {portTile}");
    }

    CoverageArea BuildCoverageWorldAbs(List<Vector2Int> relPositions, int originX, int originY, int blueprintHeight)
    {
        if (relPositions == null || relPositions.Count == 0)
            return null;

        // ✅ Convert blueprint tiles to correct world tiles by accounting for top-left origin
        var worldPositions = relPositions
            .Select(p => new Vector2Int(
                originX + p.x,
                originY - blueprintHeight + 1 + p.y
            )) // 👇 FIX: Y = originY - height + 1 + p.y
            .ToList();

        int minX = worldPositions.Min(p => p.x);
        int maxX = worldPositions.Max(p => p.x);
        int minY = worldPositions.Min(p => p.y);
        int maxY = worldPositions.Max(p => p.y);

        return new CoverageArea
        {
            startX = minX,
            endX = maxX,
            startY = minY,
            endY = maxY
        };
    }




    Entity QueueEntityBuildReturn(string id, Vector3 pos)
    {
        try
        {
            var data = Library.RequestData<EntityData>(id);
            if (data == null) return null;

            var create = new EntityCreationData
            {
                EntityID = id,
                PosX = pos.x,
                PosY = pos.y,
                SyncType = SyncType.ServerInitiated,
                IsBlueprint = true,
                ModelID = "default",
                FactionID = Singleton<FactionManager>.Instance.PlayerFactionID,
                EntityFlags = EntityFlags.IsEditable
            };

            Singleton<EntityManager>.Instance.QueueCreationEvent(create);
            return null;
        }
        catch (Exception ex)
        {
            Logger.LogError("[BlueprintImporter] Build error: " + ex);
            return null;
        }
    }

    bool TryParseBlueprint(string path, out List<List<string>> grid, out int w, out int h, out Dictionary<Vector2Int, DroneRouteBlueprintData> droneData, out List<string> lines)
    {
        lines = new List<string>();
        grid = new List<List<string>>();
        droneData = new();
        w = h = 0;

        try
        {
            lines.AddRange(File.ReadAllLines(path));
            if (lines.Count < 1)
            {
                Logger.LogError("[BlueprintImporter] Empty blueprint file.");
                return false;
            }

            var dims = lines[0].Trim().TrimEnd(';').Split('x');
            if (dims.Length != 2) return false;
            w = int.Parse(dims[0]);
            h = int.Parse(dims[1]);

            for (int i = 1; i <= h; i++)
            {
                var raw = lines[i].Trim().TrimEnd(';');
                var row = raw.Split(',').Select(s => s.Trim()).ToList();
                if (row.Count != w)
                {
                    Logger.LogError($"[BlueprintImporter] Row {i} has {row.Count} tiles; expected {w}.");
                    return false;
                }

                grid.Add(row);
            }

            for (int i = h + 1; i < lines.Count; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line) || !line.Contains(":")) continue;

                int ps = line.IndexOf("(");
                int pe = line.IndexOf(")");
                if (ps < 0 || pe <= ps) continue;

                string[] coords = line.Substring(ps + 1, pe - ps - 1).Split(',');
                if (coords.Length != 2) continue;

                if (!int.TryParse(coords[0], out int x) || !int.TryParse(coords[1], out int y)) continue;
                var relPos = new Vector2Int(x, y);
                var rest = line.Substring(pe + 1).Trim();

                var meta = new DroneRouteBlueprintData();

                // Pickup
                int pickStart = rest.IndexOf("pickup=[");
                if (pickStart != -1)
                {
                    int end = rest.IndexOf("]", pickStart);
                    if (end > pickStart)
                    {
                        string raw = rest.Substring(pickStart + 8, end - pickStart - 8);
                        foreach (var part in raw.Split(new[] { ")," }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            var trimmed = part.Trim('(', ')', ' ', ',');
                            var pair = trimmed.Split(',');
                            if (pair.Length == 2 &&
                                int.TryParse(pair[0], out int px) &&
                                int.TryParse(pair[1], out int py))
                            {
                                meta.pickup.Add(new Vector2Int(px, py));
                            }
                        }
                    }
                }

                // Dropoff
                int dropStart = rest.IndexOf("dropoff=[");
                if (dropStart != -1)
                {
                    int end = rest.IndexOf("]", dropStart);
                    if (end > dropStart)
                    {
                        string raw = rest.Substring(dropStart + 9, end - dropStart - 9);
                        foreach (var part in raw.Split(new[] { ")," }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            var trimmed = part.Trim('(', ')', ' ', ',');
                            var pair = trimmed.Split(',');
                            if (pair.Length == 2 &&
                                int.TryParse(pair[0], out int dx) &&
                                int.TryParse(pair[1], out int dy))
                            {
                                meta.dropoff.Add(new Vector2Int(dx, dy));
                            }
                        }
                    }
                }

                // Resource
                int resStart = rest.IndexOf("resource=");
                if (resStart != -1)
                {
                    int end = rest.IndexOfAny(new[] { ',', ' ', ';' }, resStart + 9);
                    meta.resourceID = (end == -1)
                        ? rest.Substring(resStart + 9).Trim()
                        : rest.Substring(resStart + 9, end - resStart - 9).Trim();
                }

                // Store entry
                droneData[relPos] = meta;

                // Log it
                Logger.LogInfo($"[BlueprintImporter] Parsed drone at relPos={relPos} → pickup={meta.pickup.Count}, dropoff={meta.dropoff.Count}, resource={meta.resourceID}");
            }



            return true;
        }
        catch (Exception ex)
        {
            Logger.LogError("[BlueprintImporter] Parse error: " + ex.Message);
            return false;
        }
    }
}

// Store drone routing info extracted from [DroneDestinations]
public class DroneRouteBlueprintData
{
    public List<Vector2Int> pickup = new();
    public List<Vector2Int> dropoff = new();
    public string resourceID;
}
