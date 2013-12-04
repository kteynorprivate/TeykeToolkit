using UnityEngine;
using System.Collections;

public class GridmapCell
{
    public Vector3 center;
    public float width, depth;
    public Vector3[] SceneVerts;

    public bool valid;

    private static Vector3[] DefaultQuadVerts = new Vector3[4] {
        new Vector3(-0.5f, 0, -0.5f),   // all edges length 1
        new Vector3(-0.5f, 0, 0.5f),
        new Vector3(0.5f, 0, 0.5f),
        new Vector3(0.5f, 0, -0.5f)};

    public GridmapCell(Vector3 c, float w = 1, float d = 1)
    {
        center = c;
        width = w;
        depth = d;

        valid = true;

        SetSceneVerts();
    }
    public void SetSceneVerts()
    {
        if(SceneVerts == null) SceneVerts = new Vector3[4];
        Vector3 scale = new Vector3(width, 0, depth);
        for (int i = 0; i < DefaultQuadVerts.Length; i++)
        {
            SceneVerts[i] = DefaultQuadVerts[i];
            SceneVerts[i].Scale(scale);
            SceneVerts[i] += center;
        }
    }
}

public class gridmap : MonoBehaviour 
{
    public float cellWidth = 1;
    public float cellDepth = 1;
    public float cliffHeight = 0.5f;

    private Vector3 halfCellSize
    {
        get { return new Vector3(cellWidth / 2.0f, 0, cellDepth / 2.0f); }
    }

    public GridmapCell[] cells;
    //public GridmapCell[,] cells;

	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    public void GenerateGrid()
    {
        if (gameObject.GetComponent<Terrain>() != null)
            GenerateGrid_Terrain();

        else return;

        
    }

    private void GenerateGrid_Terrain()
    {
        int countx = (int)(gameObject.collider.bounds.size.x / cellWidth);
        int countz = (int)(gameObject.collider.bounds.size.z / cellDepth);

        Terrain terrain = gameObject.GetComponent<Terrain>() as Terrain;

        cells = new GridmapCell[countx * countz];

        for (int x = 0; x < countx; x++)
        {
            for (int z = 0; z < countz; z++)
            {
                //cells[x, z] = new GridmapCell(new Vector3(x * cellWidth, 0, z * cellDepth) + halfCellSize + gameObject.transform.position, cellWidth, cellDepth);
                //cells[x, z].center.y = terrain.SampleHeight(cells[x, z].center);
                //cells[x, z].SetSceneVerts();
                //cells[x, z].valid = CellIsSmooth(cells[x, z], terrain);

                cells[x * countx + z] = new GridmapCell(new Vector3(x * cellWidth, 0, z * cellDepth) + halfCellSize + gameObject.transform.position, cellWidth, cellDepth);
                cells[x * countx + z].center.y = terrain.SampleHeight(cells[x * countx + z].center);
                cells[x * countx + z].SetSceneVerts();
                cells[x * countx + z].valid = CellIsSmooth(cells[x * countx + z], terrain);
            }
        }
    }

    private bool CellIsSmooth(GridmapCell c, Terrain t)
    {
        // grab 4-corners + center heights
        float ch = t.SampleHeight(c.center);
        float tl = t.SampleHeight(c.center + new Vector3(-cellWidth / 2.0f, 0, cellDepth / 2.0f));
        float tr = t.SampleHeight(c.center + new Vector3(cellWidth / 2.0f, 0, cellDepth / 2.0f));
        float bl = t.SampleHeight(c.center + new Vector3(-cellWidth / 2.0f, 0, -cellDepth / 2.0f));
        float br = t.SampleHeight(c.center + new Vector3(cellWidth / 2.0f, 0, -cellDepth / 2.0f));

        // check if center diff is too big:
        if (Mathf.Abs(ch - tl) > cliffHeight ||
            Mathf.Abs(ch - tr) > cliffHeight ||
            Mathf.Abs(ch - bl) > cliffHeight ||
            Mathf.Abs(ch - br) > cliffHeight)
            return false;

        // check if top-left diff is too big:
        if (Mathf.Abs(tl - tr) > cliffHeight ||
            Mathf.Abs(tl - bl) > cliffHeight)
            return false;

        // check if bot-right diff is too big:
        if (Mathf.Abs(br - tr) > cliffHeight ||
            Mathf.Abs(br - bl) > cliffHeight)
            return false;

        return true;
    }
}
