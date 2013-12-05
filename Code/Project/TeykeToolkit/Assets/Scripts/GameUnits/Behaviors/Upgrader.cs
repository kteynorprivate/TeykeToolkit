using UnityEngine;
using System.Collections;

namespace Teyke
{
    public class Upgrader<T> : MonoBehaviour 
        where T : GameEntity
    {
        public T UpgradeType;

        /// <summary>
        /// Upgrades the current unit to the defined upgrade type.
        /// Instantiates a new entity with the definitions of UpgradeType,
        /// clones over the data that needs to persist, then destroys the original.
        /// </summary>
        public void Upgrade()
        {
            if (UpgradeType == null) return;

            T current = gameObject.GetComponent<T>();
            T upgrade = Instantiate(UpgradeType, transform.position, transform.rotation) as T;
            current.CloneData(upgrade);
            Destroy(gameObject);
        }
    }
}