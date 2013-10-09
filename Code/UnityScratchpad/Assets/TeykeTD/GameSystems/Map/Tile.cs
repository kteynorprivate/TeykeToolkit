using UnityEngine;
using System.Collections;
using System;

public class Tile : MonoBehaviour 
{
    private static float TILE_SIZE = 1;
    public static float TileSize { get { return TILE_SIZE; } }
    private static Vector2 tileSizeVector { get { return new Vector2(TILE_SIZE, TILE_SIZE); } }

    public Texture2D Texture
    {
        get
        {
            if (gameObject.renderer == null) throw new Exception();
        }
        set
        {

        }
    }

    Tile(Texture2D texture = null)
    {
        Texture = texture;
    }

	void Start () 
    {
	    
	}

	void Update () 
    {
	    
	}
}
