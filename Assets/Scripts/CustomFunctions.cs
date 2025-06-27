using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    /// <summary>
    /// Destroys all of the transform's children
    /// </summary>
    /// <param name="transform">The parent</param>
    public static void Clear(this Transform transform)
    {
        while (transform.childCount > 0)
        {
            Transform child = transform.transform.GetChild(0);
            child.SetParent(null);
            Object.Destroy(child.gameObject);
        }
    }
    public static Vector3 With(this Vector3 v, float? x = null, float? y = null, float? z = null)
    {
        v.x = x != null ? (float)x : v.x;
        v.y = y != null ? (float)y : v.y;
        v.z = z != null ? (float)y : v.z;

        return v;
    }

    public static Vector2 With(this Vector2 v, float? x = null, float? y = null)
    {
        v.x = x != null ? (float)x : v.x;
        v.y = y != null ? (float)y : v.y;

        return v;
    }
}
