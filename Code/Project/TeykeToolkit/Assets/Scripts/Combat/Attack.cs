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

	private GameEntity currentTarget;

	public Projectile projectileBase;
    
    protected PlayerNumber owner;

	void Start () 
    {
        var go = gameObject.GetComponent<GameEntity>() as GameEntity;
        if (go == null) throw new UnityException("attack script is not attached to a GameEntity.");
        owner = go.owner;

		projectileBase.transform.position = transform.position;
	}
	
	void Update () 
    {
        cooldown -= Time.deltaTime;
        if (cooldown > 0) return;

		if(TryAttack())
			cooldown = attackSpeed;
	}

	private bool TryAttack()
	{
		GameEntity target = CheckForTarget();

		if (!target) return false;

		Projectile.FireNew(projectileBase, target, Random.Range(minDamage, maxDamage));
		currentTarget = target;
		return true;
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
			float dist = (t.transform.position - transform.position).magnitude;
			if (dist <= closestRange && dist >= minRange)
			{
                closest = t;
				closestRange = dist;
				if(closest == currentTarget) break;
			}
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
