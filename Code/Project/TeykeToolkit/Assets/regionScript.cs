using UnityEngine;
using System.Collections;

public class regionScript : MonoBehaviour 
{
	public AStarPathfinder watch;
	
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(renderer.bounds.Intersects(watch.renderer.bounds))
		{
			Messenger.Invoke("region_entered");
			Messenger<regionScript>.Invoke("region_entered", this);
		}
	}
}
