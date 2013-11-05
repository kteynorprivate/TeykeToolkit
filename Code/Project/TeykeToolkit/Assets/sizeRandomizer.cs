using UnityEngine;
using System.Collections;

public class sizeRandomizer : MonoBehaviour 
{
	public RangeAttribute size;
	// Use this for initialization
	void Start () 
	{
		size = new RangeAttribute(0.25f, 2.5f);
		Messenger.RegisterListener("region_entered", RandomizeSize);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public void RandomizeSize()
	{
		float s = (Random.value * (size.max - size.min)) + size.min;
		gameObject.transform.localScale = new Vector3(s,s,s);
	}
}
