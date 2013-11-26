using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Teyke;

public class Attack : MonoBehaviour 
{
    public float minDamage;
    public float maxDamage;
    public float minRange;
    public float maxRange;

    public float attackSpeed;
    private float cooldown;
    
    protected PlayerNumber owner;

    bool AttackTarget(GameEntity t)
    {
        if (t == null) return false;
        t.ApplyDamage(Random.Range(minDamage, maxDamage));
        Debug.Log("attacted!");
        return true;
    }

	void Start () 
    {
        var go = gameObject.GetComponent<GameEntity>() as GameEntity;
        if (go == null) throw new UnityException("attack script is not attached to a GameEntity.");
        owner = go.owner;
	}
	
	void Update () 
    {
        cooldown -= Time.deltaTime;
        if (cooldown > 0) return;

        if (AttackTarget(CheckForTarget()))
            cooldown = attackSpeed;
	}
    private GameEntity CheckForTarget()
    {
        var targets = from u in unitRegistry
                      where u.owner != owner
                      select u;
        if (targets.Count() == 0) return null;

        GameEntity closest = null;
        float closestRange = maxRange;
        foreach (var t in targets)
        {
            if ((t.transform.position - transform.position).magnitude < maxRange)
                closest = t;
        }

        return closest;
    }
    
    protected static List<GameEntity> unitRegistry = new List<GameEntity>();
    public static void RegisterAttackableUnit(GameEntity u)
    {
        unitRegistry.Add(u);
    }
    public static void UnregisterAttackableUnit(GameEntity u)
    {
        unitRegistry.Remove(u);
    }
}
