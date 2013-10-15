using UnityEngine;
using System.Collections;

public static class ExtensionMethods
{
    public static Rect Offset(this Rect r, Vector2 p)
    {
        return new Rect(r.xMin + p.x, r.yMin + p.y, r.width, r.height);
    }

    public static Vector2 Position(this Rect r)
    {
        return new Vector2(r.xMin, r.yMin);
    }
}
