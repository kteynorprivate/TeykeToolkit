using UnityEngine;
using System.Collections;

namespace Teyke
{
    public class Upgrader: MonoBehaviour 
    {
        public GameEntity UpgradeType;

        /// <summary>
        /// Upgrades the current unit to the defined upgrade type.
        /// Instantiates a new entity with the definitions of UpgradeType,
        /// clones over the data that needs to persist, then destroys the original.
        /// </summary>
        public void Upgrade()
        {
            if (UpgradeType == null) return;

            GameEntity current = gameObject.GetComponent<GameEntity>();
            GameEntity upgrade = Instantiate(UpgradeType, transform.position, transform.rotation) as GameEntity;
            current.CloneData(upgrade);
            Destroy(gameObject);
        }
    }
}