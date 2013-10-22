using UnityEngine;
using System.Collections;

public class GoalPathFollower : MonoBehaviour 
{
    public GoalBasedPath path;
    public float speed = 10;

    private Vector3 prevDir = Vector3.zero;

	// Use this for initialization
	void Start () 
    {
        path.GeneratePath();	
	}
	
	// Update is called once per frame
	void Update () 
    {
        Vector3 dir = path.FollowPath(transform.position);
        if (dir.sqrMagnitude == 0)   // hit unpathable terrain
            dir -= prevDir;
        //
        //transform.Translate(path.FollowPath(transform.position) * Time.deltaTime);
        //rigidbody.AddForce(dir * Time.deltaTime);
        rigidbody.velocity += dir;
        rigidbody.velocity.Normalize();
        //rigidbody.velocity *= speed;

        prevDir = dir;
	}
}
