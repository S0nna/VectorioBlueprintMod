using System.Collections.Generic;
using UnityEngine;

namespace BlueprintMod
{
    public class BlueprintTile
    {
        public string entityName;
        public Vector3Int position;

        public BlueprintTile(string entityName, Vector3Int pos)
        {
            this.entityName = entityName;
            this.position = pos;
        }
    }

    public class Blueprint
    {
        public List<BlueprintTile> tiles = new List<BlueprintTile>();

        public void AddTile(BlueprintTile tile)
        {
            tiles.Add(tile);
        }
    }
}
