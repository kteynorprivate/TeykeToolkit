using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(AStarPathfinder))]
public class AStarPathfinderInspector : Editor 
{
	AStarPathfinder targetPathing;
	bool showPath;
	
	private void OnEnable()
    {
        targetPathing = (AStarPathfinder)target;
    }
	
	public override void OnInspectorGUI()
    {
        showPath = EditorGUILayout.Toggle("Show A* Path: ", showPath);
        
		base.OnInspectorGUI();
    }

    public void OnSceneGUI()
    {		
        if (showPath) RenderPath();

        if ((Event.current.type == EventType.MouseDrag || Event.current.type == EventType.MouseDown) &&
            Event.current.button == 0 && 
            Event.current.modifiers != EventModifiers.Alt)
        {
            RaycastHit hitinfo;
            Physics.Raycast(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition), out hitinfo, 100);
            if (hitinfo.collider == null) return;

			targetPathing.GeneratePath(targetPathing.transform.position, hitinfo.transform.position);
            Event.current.Use();

            SceneView.RepaintAll();
            Repaint();
        }
    }
	
	private void RenderPath()
	{
		if (targetPathing.Path.Count <= 1) return;
		
		Handles.DrawLine(targetPathing.transform.position, targetPathing.Path[0]);
		for(int i = 0; i < targetPathing.Path.Count - 1; i++)
		{
			Handles.DrawSolidDisc(targetPathing.Path[i], Vector3.up, 0.15f);
			Handles.DrawLine(targetPathing.Path[i], targetPathing.Path[i + 1]);
		}
		Handles.DrawSolidDisc(targetPathing.Path[targetPathing.Path.Count - 1], Vector3.up, 0.15f);
	}
}
