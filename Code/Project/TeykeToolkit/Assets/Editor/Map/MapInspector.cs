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

    bool paintTextures = true;

    public override void OnInspectorGUI()
    {
        // toggle tiles in hierarchy
        TargetMap.TilesEditable = EditorGUILayout.Toggle("Tiles visible in hierarchy:", TargetMap.TilesEditable);
        
        
        // texture selection
        paintTextures = EditorGUILayout.Foldout(paintTextures, "Paint Textures");
        if (paintTextures && TargetMap.Textures != null)
        {
            //Tools.current = Tool.None;
            TargetMap.SelectedTexture = GUILayout.SelectionGrid(TargetMap.SelectedTexture, TargetMap.Textures, 
                ((Screen.width - 10) / 64) + 1,
                //GUILayout.MaxWidth(64), 
                //GUILayout.MaxHeight(64),
                //GUILayout.MinWidth(64),
                //GUILayout.MinHeight(64),
                GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(false));
        }

        base.OnInspectorGUI();
        
        

        EditorApplication.RepaintHierarchyWindow();
    }

    public void OnSceneGUI()
    {
        Event e = Event.current;
        //HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        //HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Keyboard));

        if (e.button == 0 && (e.type == EventType.MouseDrag || e.type == EventType.MouseDown) && !(e.modifiers == EventModifiers.Alt || e.modifiers == EventModifiers.Shift))
        {

            //Ray r = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hitinfo;
            if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(e.mousePosition), out hitinfo, 100))
            {
                if (hitinfo.collider.gameObject.GetComponent<Tile>() != null)
                {
                    Undo.RegisterUndo(hitinfo.collider.gameObject.renderer, "apply tile texture");

                    // TODO: Figure out how to fix this.
                    Material mat = new Material(hitinfo.collider.gameObject.renderer.sharedMaterial);
                    mat.mainTexture = TargetMap.Textures[TargetMap.SelectedTexture];
                    //hitinfo.collider.gameObject.renderer.sharedMaterial.mainTexture = TargetMap.Textures[TargetMap.SelectedTexture];
                    hitinfo.collider.gameObject.renderer.material = mat;

                    e.Use();
                }
            }
        }
    }
}