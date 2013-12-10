using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Teyke
{
    public class playerController : MonoBehaviour
    {
        private CharacterController cCntrlr;
        private float moveAngle;
        public float speed = 4;

        private Queue<Vector3> path = new Queue<Vector3>();
        private Vector3 nextPosition;
        private Gridmap activemap;

        // Use this for initialization
        void Start()
        {
            cCntrlr = gameObject.GetComponent<CharacterController>();
            activemap = FindObjectOfType<Gridmap>();
            nextPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                RaycastHit hitinfo;
                Camera c = GetComponentInChildren<Camera>();
                if(Physics.Raycast(c.ScreenPointToRay(Input.mousePosition), out hitinfo, 120))
                {
                    path = new Queue<Vector3>(activemap.FindPath(transform.position, hitinfo.point, true));
                }
            }

            Move();
        }
        private void Move()
        {
            //if (nextPosition == null || (path == null || path.Count == 0)) return;

            if (ReachedPoint(transform.position, nextPosition, cCntrlr.radius * cCntrlr.radius))
            {
                nextPosition = path.Count > 0 ? path.Dequeue() : transform.position;
                //nextPosition = path.Dequeue();
            }

            Vector3 dir = (nextPosition - transform.position).normalized;
            dir = transform.TransformDirection(dir);
            
            cCntrlr.Move(dir * speed);
        }
        private bool ReachedPoint(Vector3 pos, Vector3 tar, float radSqr)
        {
            Vector3 diff = tar - pos;
            diff.y = 0;

            return diff.sqrMagnitude <= radSqr;
        }
    }
}