using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetedMovementPathing : MonoBehaviour 
{
    public Map map;
    public Tile target;
    public float speed;

    private List<Tile> path;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(path.Count <= 0) return;

        transform.Translate((path[0].transform.position - transform.position).normalized * speed);

        if ((path[0].transform.position - transform.position).sqrMagnitude <= 0.5f)
            path.RemoveAt(0);
	}

    public void SetPath(Tile t)
    {
        target = t;
    }
    private void BuildPath()
    {

    }
}
