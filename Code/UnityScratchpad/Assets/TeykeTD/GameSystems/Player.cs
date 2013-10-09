using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour 
{
    /// <summary>
    /// All units in the game that belong to this player
    /// </summary>
    public List<Unit> Units;
    /// <summary>
    /// All other players who are allied with this player
    /// </summary>
    public List<Player> Allies;
    /// <summary>
    /// All other players who are enemies with this player
    /// </summary>
    public List<Player> Enemies;

    public IEnumerable<GameObject> GetTargetableUnits()
    {
        foreach (Unit unit in Units)
            if (unit.Targetable)
                yield return unit.gameObject;
    }

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}