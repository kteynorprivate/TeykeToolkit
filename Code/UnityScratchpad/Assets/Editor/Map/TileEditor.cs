using UnityEditor;
using UnityEngine;
using System.Collections;

[CanEditMultipleObjects]
[CustomEditor(typeof(Tile))]
public class TileEditor : Editor 
{
    private static Quaternion flat_quat = Quaternion.AngleAxis(90, new Vector3(1, 0, 0));

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        Object texture = EditorGUILayout.ObjectField((target as Tile).Texture, typeof(Texture2D), true);

        foreach (object t in targets)
        {
            if (t.GetType() == typeof(Tile))
            {
                (t as Tile).Texture = (Texture2D)texture;
                (t as Tile).gameObject.renderer.material.mainTexture = (t as Tile).Texture;
            }
        }
    }

    public void OnSceneGUI()
    {
        Tile tile = (Tile)target;
        Handles.color = Color.green;
        Handles.RectangleCap(0, tile.gameObject.transform.position, flat_quat, Tile.TileSize);
    }
}
