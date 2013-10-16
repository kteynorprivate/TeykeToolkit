using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(Tile))]
public class TileInspector : Editor
{
    public override void OnInspectorGUI()
    {
        Tools.current = Tool.None;

        // let user paint textures on tiles if the selected texture isn't null
        if (MapTilePreview.SelectedTile != null)
        {
        }
    }
}