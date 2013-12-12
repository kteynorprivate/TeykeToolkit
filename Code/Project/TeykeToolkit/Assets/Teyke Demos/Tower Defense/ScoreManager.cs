using UnityEngine;
using System.Collections;
using Teyke;

public class ScoreManager : MonoBehaviour 
{
    public int lives;
    public int score;

	// Use this for initialization
	void Start () 
    {
        Messenger<GameEntity>.RegisterListener("UnitReachedTarget", RemoveLive);
        Messenger<GameEntity, float>.RegisterListener("UnitDied", AddScore);
	}

    public void AddScore(GameEntity unit, float bounty)
    {
        score += (int)bounty;
    }
    public void RemoveLive(GameEntity unit)
    {
        lives--;
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 200, 60));

        GUILayout.Label("Lives: " + lives);
        GUILayout.Label("Score: " + score);

        GUILayout.EndArea();
    }
}
