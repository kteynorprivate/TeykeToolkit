using UnityEngine;
using System.Collections;

namespace Teyke
{
    public class GameStructure : GameEntity
    {
		public GameStructure UpgradeStructure;
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

		public override void Upgrade()
		{
			if (UpgradeStructure == null) return;
			
			GameStructure upgrade = Instantiate(UpgradeStructure, transform.position, transform.rotation) as GameStructure;
			CloneData(upgrade);
			upgrade.tile = tile;
			Destroy(gameObject);
		}
    }
}