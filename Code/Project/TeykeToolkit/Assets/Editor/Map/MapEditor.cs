using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[CustomEditor(typeof(Map))]
//public class MapEditor : Editor
//{
//    private static Quaternion flat_quat = Quaternion.AngleAxis(90, new Vector3(1, 0, 0));

//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//    }

//    public void OnSceneGUI()
//    {
//        Map map = (Map)target;

//        Handles.color = Color.green;

//        foreach (GameObject t in map.Tiles)
//        {
//            Handles.RectangleCap(0, t.transform.position, flat_quat, 0.5f);
//        }
//    }
//}

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

    public static List<Texture2D> Textures;

    [MenuItem("Window/Map Editor")]
    static void Init()
    {
        MapEditor window = EditorWindow.GetWindow<MapEditor>();
        Textures = new List<Texture2D>();
    }
    
    void OnGUI()
    {
        // toggle the tiles to editable or not.
        bool showMapTiles = !MapInstance.TilesEditable;
        showMapTiles = EditorGUILayout.Toggle("Lock Map Tiles", showMapTiles);
        if (showMapTiles == MapInstance.TilesEditable)
            MapInstance.TilesEditable = !showMapTiles;

        EditorGUILayout.BeginVertical();
        // selector for the map textures
        // http://docs.unity3d.com/Documentation/ScriptReference/EditorGUI.DrawPreviewTexture.html
        int x = 0;
        int y = 60;
        foreach (Texture2D tex in Textures)
        {
            Rect textureRect = new Rect(x, y, 100, 100);
            EditorGUI.DrawPreviewTexture(textureRect, tex);
            ObjectNames.GetDragAndDropTitle(tex);

            Event e = Event.current;
            if(textureRect.Offset(position.Position()).Contains(e.mousePosition) && 
                (e.type == EventType.MouseDown || e.type == EventType.MouseDrag))
            {
                DragAndDrop.StartDrag(tex.name);
                e.Use();
            }

            x += 100;
            if ((x + 100) >= this.position.width)
            {
                y += 100;
                x = 0;
            }
        }

        Object newTexture = null;
        newTexture = EditorGUI.ObjectField(new Rect(x, y, 100, 100), newTexture, typeof(Texture2D), false);
        if (newTexture != null)
        {
            Textures.Add(newTexture as Texture2D);
        }

        EditorGUILayout.EndVertical();
    }
}