using UnityEngine;
using System.Collections;

namespace Teyke
{
    public abstract class GameStructure : GameEntity
    {
        public string[] unitsProduced;
        public Tile tile;

        void Start()
        {
            if (tile == null) return;

            transform.position = tile.transform.position;
        }
        void Update()
        {

        }

        public void SetTile(Tile t)
        {
            tile = t;
            transform.position = tile.transform.position;
        }
    }
}