using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(Map))]
public class MapEditor : Editor 
{
    private static Quaternion flat_quat = Quaternion.AngleAxis(90, new Vector3(1, 0, 0));

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    public void OnSceneGUI()
    {
        Map map = (Map)target;
        Handles.color = Color.green;

        foreach (GameObject t in map.Tiles)
        {
            Handles.RectangleCap(0, t.transform.position, flat_quat, 0.5f);
        }
    }

    [MenuItem("Map/Create Map")]
    public static void CreateMap()
    {
        GameObject map = new GameObject("map");
        map.AddComponent<Map>();
        map.GetComponent<Map>().GenerateMap(16, 16);
        //Instantiate(map);
    }
}
