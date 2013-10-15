using UnityEditor;
using UnityEngine;
using System.Collections;

// TODO: Rename file to "MapMenuBar" or something.

public class MapCreationDialog : EditorWindow 
{
    static bool isVisible;
    static bool showMapTiles;
    static bool showNewMapWarning;
    int width = 16, height = 16;

    [MenuItem("Map/New Map")]
    static void ShowNewMapDialog()
    {
        if (isVisible) return;  // dont allow a second dialog to be opened.

        MapCreationDialog window = ScriptableObject.CreateInstance<MapCreationDialog>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 100);
        window.ShowPopup();
        showNewMapWarning = Map.ExistsInScene;
        isVisible = true;
    }

    [MenuItem("Map/Delete All Maps")]
    static void DeleteAllMaps()
    {
        foreach (Object o in FindObjectsOfType(typeof(GameObject)))
        {
            if ((o as GameObject).GetComponent<Map>() != null)
            {
                DestroyImmediate(o);
            }
        }
    }


    public void OnHierarchyChange()
    {
        Debug.Log("Hierarchy Changed");
    }

    void OnGUI()
    {
        width = EditorGUILayout.IntSlider("Width: ", width, 1, 64);
        height = EditorGUILayout.IntSlider("Height: ", height, 1, 64);

        if (showNewMapWarning)
        {
            EditorGUILayout.LabelField("Warning: A map object has already been created. Creating a new one will overwrite the existing one.");
        }

        if (GUILayout.Button("Create"))
        {
            GameObject map = new GameObject("map");
            map.AddComponent<Map>();
            map.GetComponent<Map>().GenerateMap(width, height);
            isVisible = false;
            this.Close();
        }
        else if (GUILayout.Button("Cancel"))
        {
            isVisible = false;
            this.Close();
        }
    }
}
