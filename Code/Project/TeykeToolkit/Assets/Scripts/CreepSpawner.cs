using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Teyke
{
    public class CreepSpawner : MonoBehaviour
    {
        private float cooldown;
        public float Interval = 5;
        public PlayerNumber player;
        public GameEntity creepType;
        public Transform creepTarget;
        private Queue<Vector3> creepPath;

        // Use this for initialization
        void Start()
        {
            cooldown = Interval;
            renderer.enabled = false;

            Gridmap map = FindObjectOfType<Gridmap>();
            creepPath = new Queue<Vector3>(map.FindPath(transform.position, creepTarget.position, true));
        }

        // Update is called once per frame
        void Update()
        {
            cooldown -= Time.deltaTime;

            if (cooldown <= 0)
            {
                SpawnCreep();
                cooldown = Interval;
            }
        }

        private void SpawnCreep()
        {
            GameEntity newCreep = Instantiate(creepType, gameObject.transform.position, Quaternion.identity) as GameEntity;
            TargetFollower creepMovement = newCreep.GetComponent<TargetFollower>();
            newCreep.owner = player;
            //Attack.RegisterAttackableUnit(newCreep);

            if (creepMovement != null && creepTarget != null)
            {
                creepMovement.SetPath(creepPath);
                creepMovement.target = creepTarget;
            }
        }
    }
}