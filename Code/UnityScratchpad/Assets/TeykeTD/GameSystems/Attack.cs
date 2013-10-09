using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attack : MonoBehaviour
{
    public float minDamage, maxDamage;
    public float range = 10f;

    float GetAttackDamage()
    {
        return Random.Range(minDamage, maxDamage); 
    }
}
