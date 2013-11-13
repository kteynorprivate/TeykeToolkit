using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Teyke
{
    public class UnitEditor : EditorWindow
    {
        bool showUnits;
        Vector2 unitScrollPosition;
        GameUnit newGameUnit;
        string newGameUnitKey;

        bool showStructures;
        Vector2 structureScrollPosition;
        GameStructure newGameStructure;

        [MenuItem("Window/Unit Editor")]
        static void ShowWindow()
        {
            EditorWindow.GetWindow<UnitEditor>();
        }

        void OnGUI()
        {
            showUnits = EditorGUILayout.Foldout(showUnits, "Game Units");
            if (showUnits)
            {
                RenderNewGameUnitArea();
                RenderGameUnitsList();
            }

            showStructures = EditorGUILayout.Foldout(showStructures, "Game Structures");
            if (showStructures)
            {
                RenderGameStructuresList();
            }
        }
        void RenderNewGameUnitArea()
        {
            EditorGUILayout.BeginHorizontal();

            newGameUnitKey = EditorGUILayout.TextField(newGameUnitKey);
            newGameUnit = EditorGUILayout.ObjectField(newGameUnit, typeof(GameUnit), false) as GameUnit;
            if (GUILayout.Button("Add") && newGameUnit != null && !string.IsNullOrEmpty(newGameUnitKey))
            {
                if (!GameEntities.GetInstance().Units.ContainsKey(newGameUnitKey) && !GameEntities.GetInstance().Units.ContainsValue(newGameUnit))
                {
                    GameEntities.GetInstance().Units.Add(newGameUnitKey, newGameUnit);
                }
            }

            EditorGUILayout.EndHorizontal();
        }
        void RenderGameUnitsList()
        {
            unitScrollPosition = EditorGUILayout.BeginScrollView(unitScrollPosition, false, true);
            foreach (KeyValuePair<string, GameUnit> u in GameEntities.GetInstance().Units)
            {
                EditorGUILayout.BeginHorizontal();
                string key = EditorGUILayout.TextField(u.Key);
                GameUnit value = EditorGUILayout.ObjectField(u.Value, typeof(GameUnit), false) as GameUnit;
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }

        void RenderGameStructuresList()
        {
            structureScrollPosition = EditorGUILayout.BeginScrollView(structureScrollPosition, false, true);
            foreach (KeyValuePair<string, GameStructure> s in GameEntities.GetInstance().Structures)
            {
                EditorGUILayout.LabelField(s.Key);
            }
            EditorGUILayout.EndScrollView();
        }
    }
}