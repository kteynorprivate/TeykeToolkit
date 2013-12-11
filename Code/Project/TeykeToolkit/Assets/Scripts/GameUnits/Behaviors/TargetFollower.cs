using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Teyke
{
    public class TargetFollower : MonoBehaviour
    {
        public float MoveSpeed;
        public Transform target;

        private static float nodeRangeSqr = 1;
        private static float repathTimeout = 0.5f;
        private float timeout;
        private Gridmap activemap;
        private Queue<Vector3> path;

        void Start()
        {
            activemap = FindObjectOfType<Gridmap>();
        }

        void Update()
        {
            if (timeout > 0) timeout -= Time.deltaTime;
            else Repath();

            FollowPath();
        }

        private void FollowPath()
        {
            if (path == null || path.Count == 0) return;

            gameObject.transform.LookAt(path.Peek());
            gameObject.transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);

            if ((transform.position - path.Peek()).sqrMagnitude <= nodeRangeSqr)
            {
                path.Dequeue();
            }
        }

        private void Repath()
        {
            timeout = repathTimeout;
            
            if (target == null) return;
            
            path = new Queue<Vector3>(activemap.FindPath(transform.position, target.position, true));
            
        }

        public void SetPath(Queue<Vector3> p)
        {
            path = p;
        }
        public void SetTimeout(float t)
        {
            timeout = t;
        }
    }
}