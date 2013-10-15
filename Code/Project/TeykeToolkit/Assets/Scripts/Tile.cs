using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public static Mesh CreateTileMesh(float width = 1, float height = 1)
    {
        Mesh m = new Mesh();
        m.name = "TileMesh";
        m.vertices = new Vector3[] { 
            new Vector3(-width, 0, -height),
            new Vector3(width, 0, -height),
            new Vector3(width, 0, height),
            new Vector3(-width, 0, height) };
        m.uv = new Vector2[] { 
            new Vector2(0,0),
            new Vector2(0,1),
            new Vector2(1,1),
            new Vector2(1,0) } ;
        m.triangles = new int[] { 0, 1, 2, 0, 2, 3 };

        m.RecalculateNormals();

        return m;
    }
}