using UnityEngine;
using System.Collections;

namespace Teyke
{
    // TODO: implement footprint-- allow larger than 1x1 cells for structure placement.

    public class GameStructure : GameEntity
    {
        public Tile tile;

        void Start()
        {
            if (tile == null) return;

            transform.position = tile.transform.position;
        }
    }
}