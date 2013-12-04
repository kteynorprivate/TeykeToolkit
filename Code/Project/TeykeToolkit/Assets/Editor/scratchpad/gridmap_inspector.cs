﻿using UnityEditor;
using UnityEngine;
using System.Collections;
using Teyke;
using System.Collections.Generic;

[CustomEditor(typeof(gridmap))]
public class gridmap_inspector : Editor 
{
    gridmap selected;
    gridmap Selected
    {
        get
        {
            if (selected == null)
                selected = (gridmap)target;
            return selected;
        }
    }

    static bool showValidCells;
    static bool showInvalidCells;

    static bool togglePathable;

    static List<Vector3> path;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        bool requestRepaint = false;

        if (Selected.cells != null)
        {
            bool vCells = GUILayout.Toggle(showValidCells, "Show Valid Cells");
            bool ivCells = GUILayout.Toggle(showInvalidCells, "Show Invalid Cells");

            if ((vCells != showValidCells) || (ivCells != showInvalidCells))
                requestRepaint = true;

            showValidCells = vCells;
            showInvalidCells = ivCells;

            togglePathable = GUILayout.Toggle(togglePathable, "Edit Pathing");
        }

        if (GUILayout.Button("Generate Grid"))
        {
            Selected.GenerateGrid();
        }

        if (requestRepaint) SceneView.RepaintAll();
    }

    public void OnSceneGUI()
    {
        if (togglePathable)
            PaintCellPathable();
        RenderCellOutlines();
    }
    private void RenderCellOutlines()
    {
        if (Selected == null) return;
        if (Selected.cells == null) return;
        if (!showValidCells && !showInvalidCells) return;

        Color c = new Color(0, 0, 0, -0.85f);
        foreach (GridmapCell cell in Selected.cells)
        {
            if (cell == null) continue;
            if (cell.valid && showValidCells) Handles.DrawSolidRectangleWithOutline(cell.SceneVerts, Color.green + c, Color.green);
            else if (!cell.valid && showInvalidCells) Handles.DrawSolidRectangleWithOutline(cell.SceneVerts, Color.red + c, Color.red);
        }

        if (path == null || path.Count <= 1) return;
        for (int i = 1; i < path.Count; i++)
        {
            Handles.DrawLine(path[i], path[i - 1]);
        }
    }
    private void PaintCellPathable()
    {
        Event e = Event.current;

        Tools.current = Tool.View;
        Tools.viewTool = ViewTool.FPS;

        if (e.button == 0 &&
            (e.type == EventType.MouseDrag || e.type == EventType.MouseDown) &&
            !(e.modifiers == EventModifiers.Alt || e.modifiers == EventModifiers.Shift))
        {
            RaycastHit hitinfo;

            if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(e.mousePosition), out hitinfo, 1000))
            {
                if (hitinfo.collider.gameObject.GetComponent<Terrain>() != null)
                {
                    GridmapCell c = Selected.GetCellFromPoint(hitinfo.point);

                    if (c == null) return;

                    Undo.RecordObject(c, "Change cell pathing.");
                    //if(c.valid == (e.modifiers != EventModifiers.Control)) return;

                    c.valid = (e.modifiers != EventModifiers.Control);//!c.valid;
                    e.Use();
                }
            }
        }
        else if (e.button == 1)
        {
            RaycastHit hitinfo;

            if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(e.mousePosition), out hitinfo, 1000))
            {
                if (hitinfo.collider.gameObject.GetComponent<Terrain>() != null)
                {
                    GridmapCell c = Selected.GetCellFromPoint(hitinfo.point);

                    if (c == null) return;

                    path = Selected.FindPath(new Vector3(0, 0, 0), c.center, true);
                }
            }
        }
    }
}
