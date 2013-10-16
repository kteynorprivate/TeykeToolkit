using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(Map))]
public class MapInspector : Editor
{
    Map targetMap;
    Map TargetMap
    {
        get
        {
            if (targetMap == null)
                targetMap = (Map)target;

            return targetMap;
        }
    }

    bool showTextures = true;

    //bool painting = false;

    public override void OnInspectorGUI()
    {
        Tools.current = Tool.None;

        // toggle tiles in hierarchy
        TargetMap.TilesEditable = EditorGUILayout.Toggle("Tiles visible in hierarchy:", TargetMap.TilesEditable);
        
        
        // texture selection
        showTextures = EditorGUILayout.Foldout(showTextures, "Tile Textures");
        if(showTextures)
        {
            TargetMap.SelectedTexture = GUILayout.SelectionGrid(TargetMap.SelectedTexture, TargetMap.Textures, ((Screen.width - 10) / 128) + 1, 
                GUILayout.MaxWidth(128), 
                GUILayout.MaxHeight(128),
                GUILayout.ExpandWidth(false),
                GUILayout.ExpandHeight(true));
        }

        base.OnInspectorGUI();
        
        EditorApplication.RepaintHierarchyWindow();
    }

    public void OnSceneGUI()
    {
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 1)
        {
            Debug.Log(HandleUtility.GUIPointToWorldRay(e.mousePosition));
        }

        if (e.type == EventType.MouseDrag || (e.type == EventType.MouseDown && e.button == 0))
        {
            Ray r = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            foreach (GameObject tile in TargetMap.Tiles)
            {
                RaycastHit hitinfo;
                if (Physics.Raycast(r, out hitinfo, 100))
                {
                    if (hitinfo.collider.gameObject.GetComponent<Tile>() != null)
                    {
                        hitinfo.collider.gameObject.renderer.material.mainTexture = TargetMap.Textures[TargetMap.SelectedTexture];
                    }
                    break;
                }
            }
            e.Use();
        }
    }
}