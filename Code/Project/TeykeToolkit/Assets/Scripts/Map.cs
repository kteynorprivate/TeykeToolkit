using UnityEditor;
using UnityEngine;
using System.Collections;

public sealed class MapTilePreview
{
    public static MapTilePreview SelectedTile;
    public static float PreviewSize = 75;
    public Texture2D Texture;
    public Rect PreviewPosition;

    public bool IsSelectedTile
    {
        get { return MapTilePreview.SelectedTile == this; }
    }
}

public sealed class Map : MonoBehaviour
{
    public GameObject[] Tiles;
    public int Width;
    public int Height;

    public Texture2D[] Textures;
    public int SelectedTexture;

    private bool tilesEditable;
    public bool TilesEditable
    {
        get { return tilesEditable; }
        set
        {
            if (value)
            {
                for (int r = 0; r < Height; r++)
                {
                    for (int c = 0; c < Width; c++)
                    {
                        GameObject t = Tiles[(r * Width) + c];
                        t.hideFlags = HideFlags.None;
                        t.transform.hideFlags = HideFlags.NotEditable;
                        t.transform.hideFlags = HideFlags.HideInInspector;
                        t.renderer.hideFlags = HideFlags.HideInInspector;
                        t.collider.hideFlags = HideFlags.HideInInspector;
                        t.GetComponent<MeshFilter>().hideFlags = HideFlags.HideInInspector;
                    }
                }
            }
            else
            {
                for (int r = 0; r < Height; r++)
                {
                    for (int c = 0; c < Width; c++)
                    {
                        GameObject t = Tiles[(r * Width) + c];
                        t.transform.hideFlags = HideFlags.NotEditable;
                        t.transform.hideFlags = HideFlags.HideInInspector;
                        t.renderer.hideFlags = HideFlags.HideInInspector;
                        t.collider.hideFlags = HideFlags.HideInInspector;
                        t.GetComponent<MeshFilter>().hideFlags = HideFlags.HideInInspector;
                        t.hideFlags = HideFlags.HideInHierarchy;
                    }
                }
            }

            tilesEditable = value;
        }
    }

    /// <summary>
    /// Determines whether or not the scene already contains a GameObject with a Map attached to it.
    /// </summary>
    public static bool ExistsInScene
    {
        get
        {
            foreach (var go in FindObjectsOfType(typeof(GameObject)))
            {
                if ((go as GameObject).GetComponent<Map>() != null)
                    return true;
            }

            return false;
        }
    }
    /// <summary>
    /// Returns the instance of the Map which exists in the scene. If there isn't one, returns null.
    /// </summary>
    public static Map SceneMapInstance
    {
        get
        {
            foreach (var go in FindObjectsOfType(typeof(GameObject)))
            {
                if ((go as GameObject).GetComponent<Map>() != null)
                    (go as GameObject).GetComponent<Map>();
            }

            return null;
        }
    }

    Map(int w = 16, int h = 16)
    {
        Width = w;
        Height = h;
    }

    public void GenerateMap(int width, int height)
    {
        gameObject.transform.hideFlags = HideFlags.HideInInspector;
        //gameObject.hideFlags = HideFlags.NotEditable;

        Width = width;
        Height = height;
        GenerateTiles();
    }
    public void GenerateTiles()
    {
        Tiles = new GameObject[Height * Width];

        float xoffset = (-Width / 2.0f) + 0.5f;
        float zoffset = (-Height / 2.0f) + 0.5f;

        for (int r = 0; r < Height; r++)
        {
            for (int c = 0; c < Width; c++)
            {
                Tiles[(r * Width) + c] = GameObject.CreatePrimitive(PrimitiveType.Quad);
                GameObject tile = Tiles[(r * Width) + c];
                tile.AddComponent<Tile>();
                tile.name = "tile_" + r + "_" + c;
                tile.transform.Rotate(90, 0, 0);
                tile.transform.position = new Vector3(xoffset, 0, zoffset);
                tile.transform.parent = this.transform;
                tile.transform.hideFlags = HideFlags.HideInInspector;
                tile.renderer.hideFlags = HideFlags.HideInInspector;
                tile.collider.hideFlags = HideFlags.HideInInspector;
                tile.GetComponent<MeshFilter>().hideFlags = HideFlags.HideInInspector;
                tile.hideFlags = HideFlags.HideInHierarchy;
                tile.isStatic = true;

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
