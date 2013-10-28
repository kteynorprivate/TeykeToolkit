using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(Tile))]
public class TileInspector : Editor
{
    public override void OnInspectorGUI()
    {
        Tools.current = Tool.None;
        
        base.OnInspectorGUI();
    }
}