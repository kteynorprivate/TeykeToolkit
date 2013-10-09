using UnityEditor;
using UnityEngine;
using System.Collections;

public sealed class Map : MonoBehaviour 
{
    public GameObject[,] Tiles;
    public int Width;
    public int Height;

    Map(int w = 16, int h = 16)
    {
        Width = w;
        Height = h;
    }

    public void GenerateMap(int width, int height)
    {
        Width = width;
        Height = height;
        GenerateTiles();
    }
    public void GenerateTiles()
    {
        Tiles = new GameObject[Height, Width];

        float xoffset = (-Width / 2.0f) + 0.5f;
        float zoffset = (-Height / 2.0f) + 0.5f;

        for (int r = 0; r < Height; r++)
        {
            for (int c = 0; c < Width; c++)
            {
                Tiles[r, c] = GameObject.CreatePrimitive(PrimitiveType.Quad);
                Tiles[r, c].name = "tile_" + r + "_" + c;
                Tiles[r, c].transform.Rotate(90, 0, 0);
                Tiles[r, c].transform.position = new Vector3(xoffset, 0, zoffset);
                Tiles[r, c].transform.parent = this.transform;

                xoffset += 1;
            }
            xoffset = (-Width / 2.0f) + 0.5f;
            zoffset += 1;
        }
    }

    void Start () 
    {
	}

    void Update()
    {
	}
}
