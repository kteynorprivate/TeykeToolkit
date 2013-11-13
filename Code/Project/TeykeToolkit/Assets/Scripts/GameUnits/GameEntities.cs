using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Teyke
{
    public enum Player
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

    public abstract class GameEntity : MonoBehaviour
    {
        public Player owner;

        public float currentHP;
        public float maxHP;
        public float productionTime;

        public string[] upgradesInto;
        public abstract void Upgrade(int targetIndex);


        // resource costs: (ex: gold, wood, food)
        // protected int resource1 cost
        // protected int resource2 cost
        // protected int resource3 cost

        public void CloneData(GameEntity target)
        {
            target.owner = owner;
            target.currentHP = currentHP <= target.maxHP ? currentHP : target.maxHP;
        }
    }

    //public class GameEntities
    //{
    //    public static Dictionary<string, GameUnit> Units = new Dictionary<string, GameUnit>();
    //    public static Dictionary<string, GameStructure> Structures = new Dictionary<string, GameStructure>();
    //}

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
                instance = ScriptableObject.Instantiate(new GameEntities()) as GameEntities;
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