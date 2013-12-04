using UnityEditor;
using UnityEngine;
using System.Collections;

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

    bool showInvalidCells;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Selected.cells != null)
            showInvalidCells = GUILayout.Toggle(showInvalidCells, "Show Invalid Cells");

        if (GUILayout.Button("Generate Grid"))
        {
            Selected.GenerateGrid();
        }
    }

    public void OnSceneGUI()
    {
        if (Selected == null) return;
        if (Selected.cells == null) return;

        Color c = new Color(0,0,0,0);
        foreach (GridmapCell cell in Selected.cells)
        {
            if (cell.valid) Handles.DrawSolidRectangleWithOutline(cell.SceneVerts, c, Color.green);
            else if (showInvalidCells) Handles.DrawSolidRectangleWithOutline(cell.SceneVerts, c, Color.red);
        }
    }
}
