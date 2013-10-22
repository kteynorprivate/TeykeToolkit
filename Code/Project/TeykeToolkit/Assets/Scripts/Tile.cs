using UnityEngine;
using System.Collections;

public enum PathingType : int
{
    UnPathable,
    GroundOnly
}

public class Tile : MonoBehaviour 
{
    public int XIndex;
    public int YIndex;
    public PathingType pathingType;

    private static Vector3[] DefaultQuadVerts = new Vector3[4] {
        new Vector3(-0.5f, -0.5f, 0),
        new Vector3(-0.5f, 0.5f, 0),
        new Vector3(0.5f, 0.5f, 0),
        new Vector3(0.5f, -0.5f, 0)};
    public Vector3[] SceneVerts
    {
        get
        {
            Vector3[] verts = new Vector3[4];
            for (int i = 0; i < DefaultQuadVerts.Length; i++)
            {
                verts[i] = transform.TransformPoint(DefaultQuadVerts[i]);
            }
            return verts;
        }
    }
}
