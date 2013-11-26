using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Teyke;

public class CreepManager : MonoBehaviour 
{
    public Map map;
    public float creepSpeed;
    public Vector3 targetPos;
    public Vector3 spawnPos;

    public GameObject creep;
    private List<Vector3> path;

    public float spawnInterval;
    public float spawnCooldown;

	// Use this for initialization
	void Start () 
    {
        if (map == null)
            map = GameObject.FindObjectOfType<Map>();

        GeneratePath();
        spawnCooldown = spawnInterval;
	}
	
	// Update is called once per frame
	void Update () 
    {
        spawnCooldown -= Time.deltaTime;

        if (spawnCooldown <= 0)
        {
            SpawnUnit();
            spawnCooldown = spawnInterval;
        }
	}
    void SpawnUnit()
    {
        GameObject u = Instantiate(creep, spawnPos, Quaternion.identity) as GameObject;
        u.transform.parent = transform;
        Creep c = u.GetComponent<Creep>();
        c.endTarget = targetPos;
        c.speed = creepSpeed;
        c.transform.position = spawnPos;
        c.SetPath(path);
        u.GetComponent<GameUnit>().owner = PlayerNumber.NeutralHostile;
        
        Attack.RegisterAttackableUnit(u.GetComponent<GameUnit>());
    }

    public void GeneratePath()
    {
        AStarPathfinder finder = new AStarPathfinder();
        finder.map = map;
        finder.GeneratePath(spawnPos, targetPos);
        path = finder.Path;
    }
}
