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

    /// <summary>
    /// Base class for Teyke game units/structures. 
    /// </summary>
    public class GameEntity : MonoBehaviour
    {
        /// <summary>
        /// The name of the GameEntity type
        /// </summary>
        public string TypeName;

        /// <summary>
        /// The ID of the owning player
        /// </summary>
        public PlayerNumber owner;
        /// <summary> 
        /// Current HP value for the unit 
        /// </summary>
        public float currentHP;
        /// <summary> 
        /// Full HP value for the unit 
        /// </summary>
        public float maxHP;
        /// <summary> 
        /// Reward a player gets for killing this unit 
        /// </summary>
        public float bounty;

        /// <summary> 
        /// Determine if the unit's HP is > 0 
        /// </summary>
        public bool Alive
        {
            get
            {
                return currentHP > 0;
            }
        }

        /// <summary> 
        /// Amount of resource1 it takes to create this unit. 
        /// </summary>
        public int resource1_cost;
        /// <summary> 
        /// Amount of resource2 it takes to create this unit. 
        /// </summary>        
        public int resource2_cost;
        /// <summary> 
        /// Amount of resource3 it takes to create this unit. 
        /// </summary>
        public int resource3_cost;

        void Start()
        {
            Attack.RegisterAttackableUnit(this);
        }

        /// <summary>
        /// Used when upgrading-- copies the current unit's stats to the new one. 
        /// </summary>
        /// <param name="target">the GameEntity that will acquire this one's stats</param>
        public void CloneData(GameEntity target)
        {
            target.owner = owner;
            target.currentHP = target.maxHP - (maxHP - currentHP);
        }

        /// <summary> 
        /// Apply damage to the GameEntity and check whether or not it is still alive. If not, emit it's death signal to the Messenger system. 
        /// </summary>
        /// <param name="amount">the amount of damage to apply</param>
        public void ApplyDamage(float amount)
        {
            currentHP -= amount;

            if (!Alive)
            {
                Messenger<GameEntity, float>.Invoke("UnitDied", this, bounty);
                Attack.UnregisterAttackableUnit(this);
                Destroy(gameObject);
            }
        }

        public void ShowControlGUI()
        {
            // TODO: Make this more extensible. Allow other components to inject their GUI code without having to do anything weird.

            GUILayout.BeginArea(new Rect(0, Screen.height - 200, 200, 200));

            Upgrader upgrader = gameObject.GetComponent<Upgrader>();
            if(upgrader != null)
            {
                if(GUILayout.Button("Upgrade: " + upgrader.UpgradeType.resource1_cost))
                    upgrader.Upgrade();
            }

            UnitProducer producer = gameObject.GetComponent<UnitProducer>();
            if (producer != null)
            {
                // Todo: implement. expose each target unit produced in the script, iterate over each here, and pass the clicked one back to the script.
            }

            StructureBuilder builder = gameObject.GetComponent<StructureBuilder>();
            if (builder != null)
            {
                for (int i = 0; i < builder.StructuresBuilt.Length; i++)
                {
                    // TODO: find a way to redirect the next vector3 from the selection manager to the StructureBuilder (USE MESSENGER)
                    //if(GUILayout.Button(builder.StructuresBuilt[i].TypeName))
                        //builder.BuildStructure(
                }
            }

            GUILayout.EndArea();
        }

        /// <summary> 
        /// Emits a message through the Messenger system when this game entity is clicked. 
        /// Subscribe to anywhere you need to get user selection info.
        /// </summary>
        public void OnMouseDown()
        {
            Messenger<GameEntity>.Invoke("GameEntityPressed", this);
        }
    }

    /// <summary>
    /// Container class that holds references to all the unit and structure types in the game.
    /// Use this to build, upgrade, and train units/structures without needing to store gameobject references
    /// inside each unit itself. simply keep the key for the unit/structure and pass it into this class.
    /// </summary>
    [Serializable]
    public class GameEntities : ScriptableObject
    {
        /// <summary> 
        /// Container for all registered GameUnit types 
        /// </summary>
        public Dictionary<string, GameUnit> Units = new Dictionary<string, GameUnit>();
        /// <summary> 
        /// Container for all registered GameStructure types 
        /// </summary>
        public Dictionary<string, GameStructure> Structures = new Dictionary<string, GameStructure>();

        public void OnEnable() { hideFlags = HideFlags.HideAndDontSave; }

        private static GameEntities instance;
        /// <summary> Gets the singleton instance of the GameEntities collection </summary>
        /// <returns>the instance of the GameEntities collection object</returns>
        public static GameEntities GetInstance()
        {
            if (instance == null)
                instance = ScriptableObject.FindObjectOfType<GameEntities>();
            if (instance == null)
                instance = ScriptableObject.CreateInstance<GameEntities>();
            return instance;
        }

        /// <summary> Registers a unit type to the global collection of unit types so that it can be instantiated elsewhere in the game. </summary>
        /// <param name="name">the unique name ID for the unit type</param>
        /// <param name="unit">the unit</param>
        public static void AddUnit(string name, GameUnit unit)
        {
            instance.Units.Add(name, unit);
        }
        /// <summary> Registers a structure type to the global collection of structure types so that it can be instantiated elsewhere in the game. </summary>
        /// <param name="name">the unique name ID for the structure type</param>
        /// <param name="unit">the structure</param>
        public static void AddStructure(string name, GameStructure unit)
        {
            instance.Structures.Add(name, unit);
        }
    }
}