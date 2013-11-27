using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Teyke
{
    public enum PlayerNumber
    {
        Player1,
        Player2,
        Player3,
        Player4,
        Player5,
        Player6,
        Player7,
        Player8,
        Neutral,
        NeutralHostile
    }

    public class Player
    {
        public PlayerNumber number;
        public int resource1, resource2, resource3;
    }

    public abstract class GameEntity : MonoBehaviour
    {
        public PlayerNumber owner;

        public float currentHP;
        public float maxHP;
        public float bounty;

        public bool Alive
        {
            get
            {
                return currentHP > 0;
            }
        }

        // resource costs: (ex: gold, wood, food)
        public int resource1_cost;
        public int resource2_cost;
        public int resource3_cost;

        public abstract void Upgrade();

        public void CloneData(GameEntity target)
        {
            target.owner = owner;
            target.currentHP = currentHP <= target.maxHP ? currentHP : target.maxHP;
        }

        public void ApplyDamage(float amount)
        {
            currentHP -= amount;

            if(!Alive) Messenger<GameEntity, float>.Invoke("UnitDied", this, bounty);
        }

		public void OnMouseDown()
		{
			Messenger<GameEntity>.Invoke("GameEntityPressed", this);
		}
    }

    [Serializable]
    public class GameEntities : ScriptableObject
    {
        public Dictionary<string, GameUnit> Units = new Dictionary<string, GameUnit>();
        public Dictionary<string, GameStructure> Structures = new Dictionary<string, GameStructure>();

        public void OnEnable() { hideFlags = HideFlags.HideAndDontSave; }

        private static GameEntities instance;
        public static GameEntities GetInstance()
        {
            if (instance == null)
                instance = ScriptableObject.FindObjectOfType<GameEntities>();
            if (instance == null)
				instance = ScriptableObject.CreateInstance<GameEntities> ();
            return instance;
        }

        public static void AddUnit(string name, GameUnit unit)
        {
            instance.Units.Add(name, unit);
        }
        public static void AddStructure(string name, GameStructure unit)
        {
            instance.Structures.Add(name, unit);
        }
    }
}