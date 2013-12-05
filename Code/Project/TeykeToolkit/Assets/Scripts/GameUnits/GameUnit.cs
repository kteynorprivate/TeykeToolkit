using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Teyke
{
	public enum UnitState
	{
		Idle,
		Move,
		MoveAttack,
		Attack
	}

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