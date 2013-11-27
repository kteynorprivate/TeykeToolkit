using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

public class AStarConnection
{
    public Tile tile;
    public AStarConnection parent;
    public float linkCost;
    public float totalCost;
    public float estimatedFinalCost;

    public static Vector3 target;

    public AStarConnection(Tile t, AStarConnection p, float lcost)
    {
        tile = t;
        parent = p;
        linkCost = lcost;

        totalCost = (parent != null) ? parent.totalCost + linkCost : 0;
        estimatedFinalCost = (tile != null) ? totalCost + (target - tile.transform.position).magnitude : float.MaxValue;
    }
    public void Reparent(AStarConnection p, float lcost)
    {
        parent = p;
        linkCost = lcost;

        totalCost = p.totalCost + linkCost;
        estimatedFinalCost = totalCost + (target - tile.transform.position).magnitude;
    }
}

public class AStarPathfinder : MonoBehaviour 
{
	public bool autoPath;
	
    public Map map;
	
    public float Speed = 1.0f;
    
    public List<AStarConnection> OpenList = new List<AStarConnection>(); 
    public List<AStarConnection> ClosedList = new List<AStarConnection>();

    public List<Vector3> Path = new List<Vector3>();

	void Start () 
    {
        if (map == null) map = Map.SceneMapInstance;
		
		map.MapModified += RegeneratePath;
        //OpenList = new List<AStarConnection>();
        //ClosedList = new List<AStarConnection>();
        //Path = new List<Vector3>();
	}
	
	void Update ()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hitinfo;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitinfo, 100);
            if (hitinfo.collider == null) return;
			
            GeneratePath(transform.position, hitinfo.transform.position);
        }
		
		if(autoPath && Path.Count == 0)
		{
			GeneratePath(transform.position, new Vector3((Random.value * map.Width) - (map.Width / 2), transform.position.y, (Random.value * map.Height) - (map.Height / 2)));
		}

        UpdatePath();
	}
    private void UpdatePath()
    {
        if (Path.Count == 0)
            return;

        transform.Translate((Path[0] - transform.position).normalized * Speed * Time.deltaTime);

        if ((transform.position - Path[0]).sqrMagnitude < 0.15f)
            Path.RemoveAt(0);
    }

    public void GeneratePath(Vector3 cpos, Vector3 tpos)
    {
        Tile current = map.ClosestTile(cpos);
        Tile target = map.ClosestTile(tpos);

        if (target.pathingType == PathingType.UnPathable) return;

        AStarConnection.target = target.transform.position;

        OpenList.Clear();
        ClosedList.Clear();

        OpenList.Add(new AStarConnection(current, null, 0));

        while (OpenList.Count > 0)
        {
            if (OpenList[0].tile.Equals(target)) break;
			
			bool up_ok = PlaceInCorrectList(OpenList[0].tile.link_Up, OpenList[0], 1.0f);
			bool left_ok = PlaceInCorrectList(OpenList[0].tile.link_Left, OpenList[0], 1.0f);
            bool right_ok = PlaceInCorrectList(OpenList[0].tile.link_Right, OpenList[0], 1.0f);
            bool down_ok = PlaceInCorrectList(OpenList[0].tile.link_Down, OpenList[0], 1.0f);
			
			if(up_ok && left_ok)
            	PlaceInCorrectList(OpenList[0].tile.link_UpLeft, OpenList[0], 1.4f);
			if(up_ok && right_ok)
            	PlaceInCorrectList(OpenList[0].tile.link_UpRight, OpenList[0], 1.4f);            
            if(down_ok && left_ok)
				PlaceInCorrectList(OpenList[0].tile.link_DownLeft, OpenList[0], 1.4f);
			if(down_ok && right_ok)
            	PlaceInCorrectList(OpenList[0].tile.link_DownRight, OpenList[0], 1.4f);

            ClosedList.Add(OpenList[0]);
            OpenList.RemoveAt(0);

            if (OpenList.Count > 0)
                OpenList.Sort((l1, l2) => l1.estimatedFinalCost.CompareTo(l2.estimatedFinalCost));
            else break;
        }

        if (OpenList.Count != 0)    // a path was found
        {
            Path.Clear();
            Path.Add(tpos);
            AStarConnection con = OpenList[0];            
            while (con.parent != null && con.parent.tile != current)
            {
                Path.Add(con.tile.transform.position);
                con = con.parent;
            }
            Path.Reverse();
        }
    }
	public void RegeneratePath()
	{
		if(Path.Count <= 0) return;
		
		GeneratePath(transform.position, Path.Last());
	}
    private bool PlaceInCorrectList(Tile t, AStarConnection c, float linkCost)
    {
        if (t == null) return false;
        if (t.pathingType == PathingType.UnPathable || t.pathingType == PathingType.AirOnly) return false;

        // check if the tile is on the open list.
        var openListInstance = OpenList.Where(conn => conn.tile.Equals(t));
        if (openListInstance.Count() > 0)   // it's on the open list.
        {
            var conn = openListInstance.First();
            if (conn.totalCost > c.totalCost + linkCost)    // this new path is shorter
            {
                conn.Reparent(c, linkCost);
            }
			
			return true;
        }

        // check if the tile is on the closed list.
        var closedListInstance = ClosedList.Where(conn => conn.tile.Equals(t));
        if (closedListInstance.Count() > 0)     // it's on the closed list.
        {
            return true;
        }

        // not on either list, add it to the open list.
        OpenList.Add(new AStarConnection(t, c, linkCost));
		return true;
    }
}

public static class AStarHelper
{
    public static List<Vector3> GeneratePath(Vector3 start, Vector3 end, Map map)
    {
        throw new UnityException();
    }
}