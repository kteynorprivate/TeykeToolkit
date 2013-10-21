using UnityEditor;
using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

using Object = UnityEngine.Object;

public enum MapInspectorTool : int
{
    None,
    Texturing,
    Pathing,
}

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
    static MapInspectorTool currentTool;

    static bool showPathingOutlines;

    private void OnEnable()
    {
        Tools.current = Tool.View;
        Tools.viewTool = ViewTool.FPS;
    }

    public override void OnInspectorGUI()
    {
        // toggle tiles in hierarchy
        // TargetMap.TilesEditable = EditorGUILayout.Toggle("Tiles visible in hierarchy:", TargetMap.TilesEditable);
        // EditorApplication.RepaintHierarchyWindow();

        // render the toolbar
        EditorGUILayout.LabelField("Current Tool");
        currentTool = (MapInspectorTool)GUILayout.Toolbar((int)currentTool, System.Enum.GetNames(typeof(MapInspectorTool)));

        switch (currentTool)
        {
            case MapInspectorTool.Texturing:
                Render_TextureSelection();
                break;
            case MapInspectorTool.Pathing:
                Render_PathingTool();
                break;
            case MapInspectorTool.None:
            default:
                break;
        }
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
    }

    public void OnSceneGUI()
    {
        switch (currentTool)
        {
            case MapInspectorTool.Texturing:
                Paint_TileMaterial();
                break;
            case MapInspectorTool.Pathing:
                break;
            case MapInspectorTool.None:
            default:
                break;
        }
    }
    private void Paint_TileMaterial()
    {
        if (TargetMap.SelectedMaterial == null) return;

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
                    Undo.RegisterUndo(hitinfo.collider.gameObject.renderer, "apply tile material");

                    hitinfo.collider.gameObject.renderer.material = TargetMap.SelectedMaterial;

                    e.Use();
                }
            }
        }
    }
}