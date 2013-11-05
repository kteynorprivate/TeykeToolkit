using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

public delegate void PathRegenerateEvent();

public sealed class Map : MonoBehaviour
{
	public event PathRegenerateEvent MapModified;
	
    public GameObject[] Tiles;
    public GameObject GetTile(int r, int c)
    {
        return Tiles[(r * Width) + c];
    }
    public GameObject this[int i]
    {
        get
        {
            return Tiles[i];
        }
        set
        {
            Tiles[i] = value;
        }
    }
    public GameObject this[int r, int c]
    {
        get
        {
            return Tiles[(r * Width) + c];
        }
        set
        {
            Tiles[(r * Width) + c] = value;
        }
    }

    public int Width;
    public int Height;

    public int SelectedMaterialIndex;
    public List<Material> TileMaterials;
    public Material SelectedMaterial
    {
        get
        {
            if (SelectedMaterialIndex < TileMaterials.Count && SelectedMaterialIndex >= 0)
                return TileMaterials[SelectedMaterialIndex];
            else return null;
        }
    }
    public Texture[] TileTextures
    {
        get
        {
            return (from mat in TileMaterials select mat.mainTexture).ToArray();
        }
    }
    public Texture SelectedTexture
    {
        get
        {
            if (SelectedMaterialIndex < TileMaterials.Count && SelectedMaterialIndex >= 0)
                return TileTextures[SelectedMaterialIndex];
            else return null;
        }
    }

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
                    return (go as GameObject).GetComponent<Map>();
            }

            return null;
        }
    }

    Map(int w = 16, int h = 16)
    {
        Width = w;
        Height = h;
        SelectedMaterialIndex = 0;
        TileMaterials = new List<Material>();
    }

	void Start()
	{
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
        Tiles = new GameObject[Width * Height];
        for (int i = 0; i < Width * Height; i++)
        {
            Tiles[i] = GameObject.CreatePrimitive(PrimitiveType.Quad);
            Tiles[i].AddComponent<Tile>();
        }

        float xoffset = (-Width / 2.0f) + 0.5f;
        float zoffset = (-Height / 2.0f) + 0.5f;

        for (int r = 0; r < Height; r++)
        {
            for (int c = 0; c < Width; c++)
            {
                //Tiles[(r * Width) + c] = GameObject.CreatePrimitive(PrimitiveType.Quad);
                GameObject tile = Tiles[(r * Width) + c];
                //tile.AddComponent<Tile>();
                Tile t = tile.GetComponent<Tile>();
                //tile.GetComponent<Tile>().XIndex = c;
                //tile.GetComponent<Tile>().YIndex = r;
                t.XIndex = c;
                t.YIndex = r;

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


                t.link_UpLeft = (r < Height - 1 && c > 0) ? Tiles[((r + 1) * Width) + (c - 1)].GetComponent<Tile>() : null;
                t.link_Up = (r < Height - 1) ? Tiles[((r + 1) * Width) + (c)].GetComponent<Tile>() : null;
                t.link_UpRight = (r < Height - 1 && c < Width - 1) ? Tiles[((r + 1) * Width) + (c + 1)].GetComponent<Tile>() : null;
                t.link_Left = (c > 0) ? Tiles[((r) * Width) + (c - 1)].GetComponent<Tile>() : null;
                t.link_Right = (c < Width - 1) ? Tiles[((r) * Width) + (c + 1)].GetComponent<Tile>() : null;
                t.link_DownLeft = (r > 0 && c > 0) ? Tiles[((r - 1) * Width) + (c - 1)].GetComponent<Tile>() : null;
                t.link_Down = (r > 0) ? Tiles[((r - 1) * Width) + (c)].GetComponent<Tile>() : null;
                t.link_DownRight = (r > 0 && c < Width - 1) ? Tiles[((r - 1) * Width) + (c + 1)].GetComponent<Tile>() : null;

                t.pathingType = PathingType.GroundOnly;

                xoffset += 1;
            }
            xoffset = (-Width / 2.0f) + 0.5f;
            zoffset += 1;
        }
    }
	
	public void Refresh()
	{
		if(MapModified != null) MapModified();
	}

    /// <summary>
    /// Gets the tile closest to the position passed in.
    /// </summary>
    public Tile ClosestTile(Vector3 position)
    {
        // TODO: OPTIMIZE THIS!!!!! Quad trees ftw.

        float shortestDistance = float.MaxValue;
        Tile closest = null;

        for (int i = 0; i < Tiles.Length; i++)
        {
            float dist = (Tiles[i].transform.position - position).sqrMagnitude;
            if (dist < shortestDistance)
            {
                shortestDistance = dist;
                closest = Tiles[i].GetComponent<Tile>();
            }
        }

        return closest;
    }
}