using UnityEngine;
using System.Collections;
using Teyke;

public class Tower_1 : GameStructure
{
    public GameStructure upgradeTarget;

	// Use this for initialization

    public override void Upgrade()
    {
        if(upgradeTarget == null) return;
        //if (owner.resource1 < upgradeTarget.resource1_cost || 
        //    owner.resource2 < upgradeTarget.resource2_cost || 
        //    owner.resource3 < upgradeTarget.resource3_cost)
        //    return;

        //owner.resource1 -= upgradeTarget.resource1_cost;
        //owner.resource2 -= upgradeTarget.resource2_cost;
        //owner.resource3 -= upgradeTarget.resource3_cost;

        GameUnit upgrade = Instantiate(upgradeTarget, transform.position, transform.rotation) as GameUnit;
        CloneData(upgrade);
        Destroy(gameObject);
    }

	// Update is called once per frame
	void Update () 
    {

	}
}
