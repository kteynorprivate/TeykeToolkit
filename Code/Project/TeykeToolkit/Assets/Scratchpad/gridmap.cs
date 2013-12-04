using UnityEngine;
using System.Collections;

public class GridmapCell
{
    public Vector3 center;
    public float width, depth;

    private static Vector3[] DefaultQuadVerts = new Vector3[4] {
        new Vector3(-0.5f, 0, -0.5f),
        new Vector3(-0.5f, 0, 0.5f),
        new Vector3(0.5f, 0, 0.5f),
        new Vector3(0.5f, 0, -0.5f)};
    public Vector3[] SceneVerts
    {
        get
        {
            Vector3[] verts = new Vector3[4];
            for (int i = 0; i < DefaultQuadVerts.Length; i++)
            {
                verts[i] = DefaultQuadVerts[i] + center;
            }
            return verts;
        }
    }
}

public class gridmap : MonoBehaviour 
{
    public float cellWidth = 1;
    public float cellDepth = 1;

    private Vector3 halfCellSize
    {
        get { return new Vector3(cellWidth / 2.0f, 0, cellDepth / 2.0f); }
    }

    public GridmapCell[,] cells;
    private static GridmapCell defaultCell = new GridmapCell();

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
        int countx = (int)(gameObject.collider.bounds.size.x / cellWidth);
        int countz = (int)(gameObject.collider.bounds.size.z / cellDepth);

        cells = new GridmapCell[countx, countz];

        for (int x = 0; x < countx; x++)
        {
            for (int z = 0; z < countz; z++)
            {
                cells[x, z] = new GridmapCell(); 
                cells[x, z].center = new Vector3(x * cellWidth, 0, z * cellDepth) + halfCellSize;
                cells[x, z].width = cellWidth;
                cells[x, z].depth = cellDepth;
            }
        }
    }
}
