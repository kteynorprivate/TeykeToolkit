using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapTilePreview
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

public class MapEditor : EditorWindow
{
    private static Map mapInstance;
    static Map MapInstance
    {
        get
        {
            if (mapInstance == null)
                mapInstance = GameObject.Find("map").GetComponent<Map>();

            return mapInstance;
        }
    }

    public static List<MapTilePreview> Textures;

    [MenuItem("Window/Map Editor")]
    static void Init()
    {
        MapEditor window = EditorWindow.GetWindow<MapEditor>();
        Textures = new List<MapTilePreview>();
    }
    
    void OnGUI()
    {
        // toggle the tiles to editable or not.
        bool lockMapTiles = !MapInstance.TilesEditable;
        lockMapTiles = EditorGUILayout.Toggle("Lock Map Tiles", lockMapTiles);
        if (lockMapTiles == MapInstance.TilesEditable)
            MapInstance.TilesEditable = !lockMapTiles;

        EditorGUILayout.BeginVertical();
        // selector for the map textures
        // http://docs.unity3d.com/Documentation/ScriptReference/EditorGUI.DrawPreviewTexture.html
        Rect newTexturePos = RealignTexturePreviews(0, 60);
        for (int i = 0; i < Textures.Count; i++)
        {
            if (Textures[i].IsSelectedTile)
            {
                EditorGUI.DrawRect(Textures[i].PreviewPosition, new Color(0,1,0));
                EditorGUI.DrawPreviewTexture(Textures[i].PreviewPosition.Expand(-2), Textures[i].Texture);
            }
            else EditorGUI.DrawPreviewTexture(Textures[i].PreviewPosition, Textures[i].Texture);
        }

        // spot to add new textures
        Object newTexture = null;
        newTexture = EditorGUI.ObjectField(newTexturePos, newTexture, typeof(Texture2D), false);
        if (newTexture != null)
        {
            AddTexture(newTexture as Texture2D);
        }
        
        // tile selection
        Event e = Event.current;
        if (e.type == EventType.MouseDown || e.type == EventType.MouseDrag)
        {
            MapTilePreview.SelectedTile = null;

            foreach (MapTilePreview mtp in Textures)
            {
                if (mtp.PreviewPosition.Contains(e.mousePosition))
                {
                    MapTilePreview.SelectedTile = mtp;
                    e.Use();
                }
            }
        }

        // make texture preview's draggable (to drag onto map tiles
        foreach (MapTilePreview t in Textures)
        {
            //Event e = Event.current;
            //if(textureRect.Offset(position.Position()).Contains(e.mousePosition) && 
            //    (e.type == EventType.MouseDown || e.type == EventType.MouseDrag))
            //{
            //    //Debug.Log(t.name);
            //    //DragAndDrop.StartDrag(t.name);
            //    e.Use();
            //}
        }

        EditorGUILayout.EndVertical();
    }

    private bool AddTexture(Texture2D t)
    {
        // check if the t's name is already taken
        foreach (MapTilePreview mtp in Textures)
        {
            if (mtp.Texture.name.Equals(t.name))
                return false;
        }

        Textures.Add(new MapTilePreview() { Texture = t });
        return true;
    }

    /// <summary>
    /// Align the texture preview rectangles to fit the dimensions of the window.
    /// </summary>
    /// <param name="xoffset">A default x offset to add to all of the tiles.</param>
    /// <param name="yoffset">A default y offset to add to all of the tiles.</param>
    /// <returns>A Rect which holds the position and dimensions right after the last texture tile.</returns>
    Rect RealignTexturePreviews(float xoffset = 0, float yoffset = 0)
    {
        for (int i = 0; i < Textures.Count; i++)
        {
            MapTilePreview t = Textures[i];
            Textures[i].PreviewPosition = new Rect(xoffset, yoffset, MapTilePreview.PreviewSize, MapTilePreview.PreviewSize);

            xoffset += MapTilePreview.PreviewSize;
            if (xoffset + MapTilePreview.PreviewSize >= position.width)
            {
                xoffset = 0;
                yoffset += MapTilePreview.PreviewSize;
            }
        }

        return new Rect(xoffset, yoffset, MapTilePreview.PreviewSize, MapTilePreview.PreviewSize);
    }
}