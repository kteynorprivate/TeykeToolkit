using UnityEngine;
using System.Collections;

public class AStarFollower : MonoBehaviour 
{
    public AStarTree PathTree;
    public AStarPath Path;

	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetMouseButtonDown(0))
            CreateNewPath();

        if (Path == null) return;

        transform.position += Path.FollowPath(transform.position) * Time.deltaTime;
	}

    private void CreateNewPath()
    {
        Camera c = (Camera)GameObject.FindObjectOfType(typeof(Camera));
        Vector3 target = PathTree.map.ClosestTile(c.ScreenToWorldPoint(Input.mousePosition)).transform.position;
        Path = PathTree.GeneratePath(transform.position, target);
    }
}
