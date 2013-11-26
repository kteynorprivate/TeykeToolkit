using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Teyke;

public class Creep : MonoBehaviour 
{
    public List<Vector3> path;
    
    public float speed;
    public Vector3 endTarget;
    private GameUnit gu;

	void Start () 
    {
        gu = gameObject.GetComponent<GameUnit>();
	}
	
	void Update () 
    {
        UpdatePath();

        if ((endTarget - transform.position).sqrMagnitude < 0.05f || !gu.Alive)
        {
            Attack.UnregisterAttackableUnit(gu);
            Messenger.Invoke("CreepPassed");
            Destroy(gameObject);
        }
	}
    void UpdatePath()
    {
        if (path.Count == 0) return;

        transform.Translate((path[0] - transform.position).normalized * speed * Time.deltaTime);
        if ((transform.position - path[0]).sqrMagnitude < 0.025f)
            path.RemoveAt(0);
    }
    public void SetPath(List<Vector3> path)
    {
        this.path = new List<Vector3>(path.Count);
        foreach (var p in path)
            this.path.Add(p);
    }
}
