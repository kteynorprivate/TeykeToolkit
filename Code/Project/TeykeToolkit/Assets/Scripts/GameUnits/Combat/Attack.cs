using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Teyke;

// TODO: Break into melee/ranged attacks
namespace Teyke
{
    public class Attack : MonoBehaviour
    {
        public float minDamage;
        public float maxDamage;
        public float minRange;
        public float maxRange;

        /// <summary>
        /// The time spent waiting between each attack.
        /// </summary>
        public float attackSpeed;
        private float cooldown;

        private GameEntity currentTarget;

        /// <summary>
        /// Instance of the projectile object that will be createad whenever the weapon is fired.
        /// </summary>
        public Projectile projectileBase;
        /// <summary>
        /// The ID of the player who owns the unit.
        /// Used to check for valid targets.
        /// </summary>
        protected PlayerNumber owner;

        void Start()
        {
            var go = gameObject.GetComponent<GameEntity>() as GameEntity;
            if (go == null) throw new UnityException("attack script is not attached to a GameEntity.");
            owner = go.owner;

            projectileBase.transform.position = transform.position;
        }

        void Update()
        {
            cooldown -= Time.deltaTime;
            if (cooldown > 0) return;

            if (TryAttack())
                cooldown = attackSpeed;
        }

        private bool TryAttack()
        {
            GameEntity target = CheckForTarget();

            if (!target) return false;
            
            Projectile.FireNew(projectileBase, transform.position, target, Random.Range(minDamage, maxDamage));
            currentTarget = target;
            return true;
        }
        private GameEntity CheckForTarget()
        {
            var targets = from u in unitRegistry
                          where u.owner != owner
                          select u;

            if (targets.Count() == 0) return null;

            GameEntity closest = null;
            float closestRange = maxRange;
            foreach (var t in targets)
            {
                float dist = (t.transform.position - transform.position).magnitude;
                if (dist <= closestRange && dist >= minRange)
                {
                    closest = t;
                    closestRange = dist;
                    if (closest == currentTarget) break;
                }
            }

            return closest;
        }


        /// <summary>
        /// Static list of all units that it is possible to attack. Units should add 
        /// and remove themselves to the registry when appropriate.
        /// </summary>
        protected static List<GameEntity> unitRegistry = new List<GameEntity>();
        /// <summary>
        /// Add a unit to the registry of attackable ones.
        /// </summary>
        /// <param name="u">The unit to add the the registry</param>
        public static void RegisterAttackableUnit(GameEntity u)
        {
            unitRegistry.Add(u);
        }
        /// <summary>
        /// Remove a unit from the registry of attackone ones.
        /// <summary>
        /// <param name="u">The unit to remove from the registry</param>
        /// <returns>True if the unit was removed, false if it wasn't found in the registry.</returns>
        public static bool UnregisterAttackableUnit(GameEntity u)
        {
            return unitRegistry.Remove(u);
        }
    }
}