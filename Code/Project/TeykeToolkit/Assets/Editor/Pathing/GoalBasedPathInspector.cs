using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(GoalBasedPath))]
public class GoalBasedPathInspector : Editor 
{
    GoalBasedPath targetPathing;
    static bool showHeatmap;
    static bool showVectorField;

    private void OnEnable()
    {
        targetPathing = (GoalBasedPath)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        showHeatmap = EditorGUILayout.Toggle("Show Heatmap: ", showHeatmap);
        showVectorField = EditorGUILayout.Toggle("Show Vector Field: ", showVectorField);

        if (GUILayout.Button("Generate Path"))
        {
            targetPathing.GeneratePath();
        }
    }

    public void OnSceneGUI()
    {
        if (showHeatmap) RenderHeatmap();
        if (showVectorField) RenderVectorField();
    }
    private void RenderHeatmap()
    {
        if (targetPathing.heatmap == null) return;
        Color heat = Color.black;

        for (int r = 0; r < targetPathing.map.Height; r++)
        {
            for (int c = 0; c < targetPathing.map.Width; c++)
            {
                if (targetPathing.heatmap[c, r] == -1) heat.a = 1;
                else heat.a = (float)targetPathing.heatmap[c, r] / (float)targetPathing.maxHeatValue;
                Handles.DrawSolidRectangleWithOutline(targetPathing.map.Tiles_MultiDim[c, r].SceneVerts, heat, heat);
            }
        }
    }
    private void RenderVectorField()
    {
        if (targetPathing.vectorField == null) return;

        for (int r = 0; r < targetPathing.map.Height; r++)
        {
            for (int c = 0; c < targetPathing.map.Width; c++)
            {
                if (targetPathing.heatmap[c, r] == 01) continue;

                Handles.DrawLine(targetPathing.map.Tiles_MultiDim[c, r].transform.position, targetPathing.map.Tiles_MultiDim[c, r].transform.position + new Vector3(targetPathing.vectorField[c, r].x, 0, targetPathing.vectorField[c, r].y));
            }
        }
    }
}
