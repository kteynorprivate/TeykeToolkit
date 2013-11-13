using UnityEngine;
using System.Collections;

public class TestUpgrader : MonoBehaviour 
{
    Teyke.GameEntity unit;
    float countdown = 5;

	// Use this for initialization
	void Start () 
    {
        countdown = 5;
        unit = gameObject.GetComponent<Teyke.GameEntity>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (countdown > 0) countdown -= Time.deltaTime;
	}

    void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 140, 30), "Upgrade in: " + countdown);
        GUI.enabled = countdown <= 0;
        if (GUI.Button(new Rect(20, 60, 80, 30), "Upgrade") && unit != null)
        {
            unit.Upgrade();
            countdown = 5;
        }
    }
}
