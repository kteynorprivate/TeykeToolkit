using UnityEngine;
using System.Collections;

namespace Teyke
{
    [ExecuteInEditMode()]
    public class GameUnit : GameEntity
    {
        private float buildTime;

        public string[] structuresBuilt;

        void Start()
        {

        }
        void Update()
        {

        }

        public override void Upgrade(int targetIndex)
        {
            if (targetIndex < 0 || targetIndex >= upgradesInto.Length) return;

            if (GameEntities.GetInstance().Units[upgradesInto[targetIndex]] == null) return;

            GameUnit upgrade = Instantiate(GameEntities.GetInstance().Units[upgradesInto[targetIndex]], transform.position, transform.rotation) as GameUnit;
            CloneData(upgrade);
            Destroy(this);
        }
    }
}