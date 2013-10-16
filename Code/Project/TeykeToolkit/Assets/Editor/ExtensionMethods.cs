using UnityEngine;
using System.Collections;

public static class ExtensionMethods
{
    public static Rect Offset(this Rect r, Vector2 p)
    {
        return new Rect(r.xMin + p.x, r.yMin + p.y, r.width, r.height);
    }
    public static Rect Expand(this Rect r, float value)
    {
        return new Rect(r.xMin - value, r.yMin - value, r.width + value + value, r.height + value + value);
    }

    public static Vector2 Position(this Rect r)
    {
        return new Vector2(r.xMin, r.yMin);
    }

    public static bool Contains(this string[] strs, string s)
    {
        for (int i = 0; i < strs.Length; i++)
            if (strs[i] == s) return true;

        return false;
    }

    public static bool MemberOf(this string s, string[] strs)
    {
        for (int i = 0; i < strs.Length; i++)
        {
            if (strs[i].Equals(s))
                return true;
        }

        return false;
    }
}
