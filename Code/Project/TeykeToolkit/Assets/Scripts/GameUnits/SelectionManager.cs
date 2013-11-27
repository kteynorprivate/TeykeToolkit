using UnityEngine;
using System.Collections;
using Teyke;

public class SelectionManager : MonoBehaviour 
{
	GameEntity currentSelection;

	// Use this for initialization
	void Start () 
	{
		Messenger<GameEntity>.RegisterListener ("GameEntityPressed", x => { currentSelection = x; Debug.Log("new selection");} );
		Messenger<Tile>.RegisterListener("TilePressed", t => { currentSelection = null; Debug.Log("no selection");} );
	}

	void OnGUI()
	{
		if (currentSelection == null) return;
		
		if (currentSelection is GameUnit)
			ShowGameUnitBehaviour ();
		else if(currentSelection is GameStructure)
			ShowGameStructureBehaviour();
	}

	void OnPostRender()
	{
		if (currentSelection == null) return;

		ShowSelectionSquare();
	}

	void ShowSelectionSquare()
	{
		// show selection square
		Vector3 p0 = currentSelection.gameObject.renderer.bounds.min;
		Vector3 p2 = currentSelection.gameObject.renderer.bounds.max;
		
		p0 -= currentSelection.transform.position;
		p2 -= currentSelection.transform.position;
		p0 *= 1.25f;
		p2 *= 1.25f;
		p0 += currentSelection.transform.position;
		p2 += currentSelection.transform.position;
		
		Vector3 p1 = new Vector3(p0.x, 0, p2.z);
		Vector3 p3 = new Vector3(p2.x, 0, p0.z);
		
		GL.Begin(GL.LINES);
		GL.Color(Color.white);
		GL.Vertex3(p0.x, 0.15f, p0.z);
		GL.Vertex3(p1.x, 0.15f, p1.z);
		GL.Vertex3(p1.x, 0.15f, p1.z);
		GL.Vertex3(p2.x, 0.15f, p2.z);
		GL.Vertex3(p2.x, 0.15f, p2.z);
		GL.Vertex3(p3.x, 0.15f, p3.z);
		GL.Vertex3(p0.x, 0.15f, p0.z);
		GL.Vertex3(p3.x, 0.15f, p3.z);
		GL.End();
	}
	void ShowGameUnitBehaviour()
	{

	}
	void ShowGameStructureBehaviour()
	{
		GameStructure structure = currentSelection as GameStructure;
		
		if(structure.UpgradeStructure)
			if(GUILayout.Button("Upgrade"))
				structure.Upgrade();
		if(GUILayout.Button("Destroy"))
			Destroy(currentSelection.gameObject);
	}
}
