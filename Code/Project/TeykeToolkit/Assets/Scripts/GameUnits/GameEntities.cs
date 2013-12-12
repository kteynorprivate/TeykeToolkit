using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Teyke
{
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