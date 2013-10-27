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
        if (Input.GetMouseButtonDown(0))
        {
            CreateNewPath();
        }
        if (path.vectorField == null || path.heatmap == null) return;

        Vector3 dir = path.FollowPath(transform.position) * Time.deltaTime;
        if (dir.sqrMagnitude == 0)   // hit unpathable terrain
            dir -= prevDir;

        Vector3 dir_x = new Vector3(dir.x, 0, 0);
        Vector3 dir_z = new Vector3(0, 0, dir.z);

        if (path.FollowPath(transform.position + dir_x).x == 0)
            dir.x = 0;
        if (path.FollowPath(transform.position + dir_z).z == 0)
            dir.z = 0;

        transform.Translate(dir);

        prevDir = dir;
	}

    private void CreateNewPath()
    {
        Camera c = (Camera)GameObject.FindObjectOfType(typeof(Camera));
        RaycastHit hitinfo;
        Physics.Raycast(c.ScreenPointToRay(Input.mousePosition), out hitinfo, 100);
        if (hitinfo.collider == null) return;

        path.GeneratePath(path.map.ClosestTile(hitinfo.transform.position));
    }
}
