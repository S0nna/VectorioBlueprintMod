// This is an optional file for future expansion.
using UnityEngine;

namespace BlueprintMod
{
    public static class BlueprintPlacer
    {
        public static void PlaceBlueprint(Blueprint blueprint)
        {
            foreach (var tile in blueprint.tiles)
            {
                // You'd want to delegate to your plugin's QueueEntityBuild here.
                // Example:
                // BlueprintModMain.Instance.QueueEntityBuild(tile.entityName, tile.position);
            }
        }
    }
}
