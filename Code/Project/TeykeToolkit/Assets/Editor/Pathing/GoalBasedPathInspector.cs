using UnityEngine;
using UnityEditor;
using System.Collections;

struct VectorFieldRenderInfo
{
    public Vector3 line_start;
    public Vector3 line_end;
}

struct HeatMapRenderInfo
{
    public float alpha;
    public Vector3[] verts;
}

[CustomEditor(typeof(GoalBasedPath))]
public class GoalBasedPathInspector : Editor 
{
    GoalBasedPath targetPathing;
    static bool showHeatmap;
    static bool showVectorField;

    static VectorFieldRenderInfo[,] vectorField;
    static HeatMapRenderInfo[,] heatMap;

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
            vectorField = null;
            heatMap = null;
        }
    }

    public void OnSceneGUI()
    {
        if (showHeatmap) RenderHeatmap();
        if (showVectorField) RenderVectorField();

        if ((Event.current.type == EventType.MouseDrag || Event.current.type == EventType.MouseDown) &&
            Event.current.button == 0 && 
            Event.current.modifiers != EventModifiers.Alt)
        {
            RaycastHit hitinfo;
            Physics.Raycast(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition), out hitinfo, 100);
            if (hitinfo.collider == null) return;

            targetPathing.GeneratePath(targetPathing.map.ClosestTile(hitinfo.transform.position));
            vectorField = null;
            heatMap = null;
            Event.current.Use();

            SceneView.RepaintAll();
            Repaint();
        }
    }
    private void RenderHeatmap()
    {
        if (targetPathing.heatmap == null) return;

        if (heatMap == null)
        {
            heatMap = new HeatMapRenderInfo[targetPathing.heatmap.GetLength(0), targetPathing.heatmap.GetLength(1)];

            for (int r = 0; r < targetPathing.map.Height; r++)
            {
                for (int c = 0; c < targetPathing.map.Width; c++)
                {
                    heatMap[r, c] = new HeatMapRenderInfo()
                    {
                        alpha = targetPathing.heatmap[r, c] == -1 ? 1 : (float)targetPathing.heatmap[r, c] / (float)targetPathing.maxHeatValue,
                        verts = targetPathing.map.Tiles[(r * targetPathing.map.Width) + c].GetComponent<Tile>().SceneVerts
                    };
                }
            }
        }

        foreach (HeatMapRenderInfo hmri in heatMap)
        {
            Handles.DrawSolidRectangleWithOutline(hmri.verts, new Color(0, 0, 0, hmri.alpha), new Color(0, 0, 0, hmri.alpha));
        }
    }
    private void RenderVectorField()
    {
        if (targetPathing.vectorField == null) return;

        if (vectorField == null)
        {
            vectorField = new VectorFieldRenderInfo[targetPathing.vectorField.GetLength(0), targetPathing.vectorField.GetLength(1)];
            for (int r = 0; r < targetPathing.map.Height; r++)
            {
                for (int c = 0; c < targetPathing.map.Width; c++)
                {
                    vectorField[r, c] = new VectorFieldRenderInfo()
                    {
                        line_start = targetPathing.map.Tiles[(r * targetPathing.map.Width) + c].transform.position,
                        line_end = targetPathing.map.Tiles[(r * targetPathing.map.Width) + c].transform.position + new Vector3(targetPathing.vectorField[r, c].x / 2, 0, targetPathing.vectorField[r, c].y / 2)
                    };
                }
            }
        }

        foreach (VectorFieldRenderInfo vfri in vectorField)
        {
            Handles.DrawLine(vfri.line_start, vfri.line_end);
        }
    }
}