using UnityEngine;
using System.Collections;

namespace Teyke
{
    [ExecuteInEditMode()]
    public class GameUnit : GameEntity
    {
        private float buildTime;

        public string[] structuresBuilt;
        public GameUnit UpgradeUnit;

        void Start()
        {

        }
        void Update()
        {

        }

        public override void Upgrade()
        {
            if (UpgradeUnit == null) return;

            GameUnit upgrade = Instantiate(UpgradeUnit, transform.position, transform.rotation) as GameUnit;
            CloneData(upgrade);
            Destroy(gameObject);
        }
    }
}