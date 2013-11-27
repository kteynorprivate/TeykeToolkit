using UnityEngine;
using System.Collections;

public enum PathingType : int
{
    UnPathable,
    Pathable,
	AirOnly
}

public class Tile : MonoBehaviour 
{
    public int XIndex;
    public int YIndex;
    public PathingType pathingType;

    public Tile link_UpLeft;
    public Tile link_Up;
    public Tile link_UpRight;
    public Tile link_Left;
    public Tile link_Right;
    public Tile link_DownLeft;
    public Tile link_Down;
    public Tile link_DownRight;

    public Tile parent;

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

    public static float PotentialFieldWeight(PathingType type)
    {
        switch (type)
        {
            case PathingType.Pathable:
                return 0;
            case PathingType.UnPathable:
            default:
                return -1;
        }
    }

    public override bool Equals(object o)
    {
        if (o.GetType() != typeof(Tile)) return false;
        return ((o as Tile).XIndex == XIndex && (o as Tile).YIndex == YIndex);
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
