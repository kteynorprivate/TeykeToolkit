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

        if (GUILayout.Button("Generate Path"))
        {
            targetPathing.GeneratePath();
        }
    }

    public void OnSceneGUI()
    {
        if (showHeatmap) RenderHeatmap();
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
}
