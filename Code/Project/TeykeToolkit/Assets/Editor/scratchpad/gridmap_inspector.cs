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

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
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
            Handles.DrawSolidRectangleWithOutline(cell.SceneVerts, c, Color.green);
        }
    }
}
