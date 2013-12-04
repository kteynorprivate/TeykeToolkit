using UnityEditor;
using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

using Object = UnityEngine.Object;

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
    static PathingType currentPathingType;
	
	static bool paintPathing;
	static bool paintMaterial;

    static bool showPathingOutlines;

    private void OnEnable()
    {
        Tools.current = Tool.View;
        Tools.viewTool = ViewTool.FPS;
    }

    public override void OnInspectorGUI()
    {
        // toggle tiles in hierarchy
        TargetMap.TilesEditable = EditorGUILayout.Toggle("Tiles visible in hierarchy:", TargetMap.TilesEditable);
        EditorApplication.RepaintHierarchyWindow();

		paintMaterial = EditorGUILayout.Toggle("Texturing", paintMaterial);		
		if(paintMaterial)
			Render_TextureSelection();

		paintPathing = EditorGUILayout.Toggle("Pathing", paintPathing);
		if(paintPathing)
			Render_PathingTool();

		SceneView.RepaintAll();
    }
    private void Render_TextureSelection()
    {
        // add new material
        Material newMaterial = (Material)EditorGUILayout.ObjectField("Add Material", null, typeof(Material), false);
        if (newMaterial != null && !TargetMap.TileMaterials.Contains(newMaterial))
            TargetMap.TileMaterials.Add(newMaterial);

        // select a material
        if (TargetMap.SelectedMaterial != null)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Selected Material: " + TargetMap.SelectedMaterial.name);
            if (GUILayout.Button("Remove"))
            {
                TargetMap.TileMaterials.Remove(TargetMap.SelectedMaterial);
            }
            GUILayout.EndHorizontal();
        }
        if (TargetMap.TileMaterials.Count > 0)
        {
            TargetMap.SelectedMaterialIndex = GUILayout.SelectionGrid(TargetMap.SelectedMaterialIndex, TargetMap.TileTextures,
                (Screen.width / 64) + 1);
        }
    }
    private void Render_PathingTool()
    {
        EditorGUILayout.LabelField("Current Pathing Type");
        currentPathingType = (PathingType)GUILayout.Toolbar((int)currentPathingType, currentPathingType.Names());
    }

    public void OnSceneGUI()
    {		
		bool useEvent = false;
		if(paintMaterial)
			if(Paint_TileMaterial())
				useEvent = true;
		if(paintPathing)
		{
			if(Paint_PathingType())
				useEvent = true;
			Render_PathingType();
		}
		
		if(useEvent) Event.current.Use();
    }
    private bool Paint_TileMaterial()
    {
        if (TargetMap.SelectedMaterial == null) return false;

        Event e = Event.current;

        if (e.button == 0 &&
            (e.type == EventType.MouseDrag || e.type == EventType.MouseDown) &&
            !(e.modifiers == EventModifiers.Alt || e.modifiers == EventModifiers.Shift))
        {
            RaycastHit hitinfo;
            if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(e.mousePosition), out hitinfo, 100))
            {
                if (hitinfo.collider.gameObject.GetComponent<Tile>() != null)
                {
                    //Undo.RegisterUndo(hitinfo.collider.gameObject.renderer, "apply tile material"
					Undo.RecordObject(hitinfo.collider.gameObject.renderer, "apply tile material");

                    hitinfo.collider.gameObject.renderer.material = TargetMap.SelectedMaterial;
                    //e.Use();
					return true;
                }
            }
        }
		return false;
    }
    private bool Paint_PathingType()
    {
        Event e = Event.current;

        if (e.button == 0 &&
            (e.type == EventType.MouseDrag || e.type == EventType.MouseDown) &&
            !(e.modifiers == EventModifiers.Alt || e.modifiers == EventModifiers.Shift))
        {
            RaycastHit hitinfo;
            if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(e.mousePosition), out hitinfo, 100))
            {
                if (hitinfo.collider.gameObject.GetComponent<Tile>() != null)
                {
					Undo.RecordObject(hitinfo.collider.gameObject.GetComponent<Tile>(), "set tile pathing type");

                    hitinfo.collider.gameObject.GetComponent<Tile>().pathingType = currentPathingType;
					targetMap.Refresh();
                    //e.Use();
					return true;
                }
            }
        }
		return false;
    }
    private void Render_PathingType()
    {
        foreach (GameObject t in TargetMap.Tiles)
        {
            Color visualColor = Color.white;
            switch (t.GetComponent<Tile>().pathingType)
            {
                case PathingType.Pathable:
                    visualColor = Color.green;
                    break;
				case PathingType.AirOnly:
					visualColor = Color.blue;
					break;
                case PathingType.UnPathable:
                    visualColor = Color.red;
                    break;
            }
            visualColor.a = 0.15f;
            Handles.DrawSolidRectangleWithOutline(t.GetComponent<Tile>().SceneVerts, visualColor, visualColor);
        }
    }
}