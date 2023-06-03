using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UnityExtensions
{
    //
    //  LayerMask
    //

    public static void Nothing(this ref LayerMask layerMask)
    {
        layerMask = 0;
    }

    public static void Everything(this ref LayerMask layerMask)
    {
        layerMask = -1;
    }

    //
    //  Vector
    //

    /// <summary>
    /// Returns a Vector2 constructed using this Vector3's x and y.
    /// </summary>
    public static Vector2 ToV2(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }

    /// <summary>
    /// Returns the X and Z components of this Vector3 as a Vector2
    /// </summary>
    public static Vector2 XZ(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }

    /// <summary>
    /// Returns a Vector2 as the result of rotating this Vector2 around origo by the provided radians.
    /// </summary>
    public static Vector2 RotateAroundZero(this Vector2 point, float radians)
    {
        return new Vector2(
            Mathf.Cos(radians) * point.x - Mathf.Sin(radians) * point.y,
            Mathf.Sin(radians) * point.x + Mathf.Cos(radians) * point.y);
    }

    /// <summary>
    /// Returns a Vector2 as the result of rotating this Vector2 around the provided pivot point by the provided radians.
    /// </summary>
    public static Vector2 RotateAroundPivot(this Vector2 point, Vector2 pivot, float radians)
    {
        return new Vector2(
            Mathf.Cos(radians) * (point.x - pivot.x) - Mathf.Sin(radians) * (point.y - pivot.y) + pivot.x,
            Mathf.Sin(radians) * (point.x - pivot.x) + Mathf.Cos(radians) * (point.y - pivot.y) + pivot.y);
    }

    //
    //  Rect
    //

    /// <summary>
    /// Returns the result of this Rect inflated by the provided amount.
    /// </summary>
    public static Rect Inflated(this Rect r, float x, float y)
    {
        return new Rect(r.x - x, r.y - y, r.width + x * 2f, r.height + y * 2f);
    }

    /// <summary>
    /// Returns the result of this RectInt inflated by the provided amount.
    /// </summary>
    public static RectInt Inflated(this RectInt r, int x, int y)
    {
        return new RectInt(r.x - x, r.y - y, r.width + x * 2, r.height + y * 2);
    }

    /// <summary>
    /// Returns whether or not the two rectangles overlap and their overlapping area.
    /// </summary>
    public static bool Overlaps(this RectInt rect, RectInt other, out RectInt overlap)
    {
        overlap = new RectInt();

        if (!rect.Overlaps(other))
            return false;

        int minX = Mathf.Max(rect.min.x, other.min.x);
        int maxX = Mathf.Min(rect.max.x, other.max.x);

        int minY = Mathf.Max(rect.min.y, other.min.y);
        int maxY = Mathf.Min(rect.max.y, other.max.y);

        Vector2Int min = new Vector2Int(minX, minY);
        Vector2Int max = new Vector2Int(maxX, maxY);

        overlap.min = min;
        overlap.max = max;
        return true;
    }

    /// <summary>
    /// Returns whether or not the two rectangles overlap and their overlapping area.
    /// </summary>
    public static bool Overlaps(this Rect rect, Rect other, out Rect overlap)
    {
        overlap = new Rect();

        if (!rect.Overlaps(other))
            return false;

        float minX = Mathf.Max(rect.min.x, other.min.x);
        float maxX = Mathf.Min(rect.max.x, other.max.x);

        float minY = Mathf.Max(rect.min.y, other.min.y);
        float maxY = Mathf.Min(rect.max.y, other.max.y);

        Vector2 min = new Vector2(minX, minY);
        Vector2 max = new Vector2(maxX, maxY);

        overlap.min = min;
        overlap.max = max;
        return true;
    }

    /// <summary>
    /// Returns whether or not this rectangle fully contains the provided rectangle
    /// </summary>
    public static bool Contains(this Rect rect, Rect other)
    {
        Rect overlap;
        return rect.Overlaps(other, out overlap) && overlap.size == other.size;
    }

    /// <summary>
    /// Returns the y coordinate for this RectInt's top side (same as yMin or y)
    /// </summary>
    public static int Top(this RectInt rect)
    {
        return rect.yMin;
    }

    /// <summary>
    /// Returns the y coordinate for this RectInt's bottom side (same as yMax or y + height)
    /// </summary>
    public static int Bottom(this RectInt rect)
    {
        return rect.yMax;
    }

    /// <summary>
    /// Returns the x coordinate for this RectInt's left side (same as xMin or x)
    /// </summary>
    public static int Left(this RectInt rect)
    {
        return rect.x + rect.width;
    }

    /// <summary>
    /// Returns the x coordinate for this RectInt's right side (same as xMax or x + width)
    /// </summary>
    public static int Right(this RectInt rect)
    {
        return rect.xMax;
    }

    /// <summary>
    /// Returns the area of this rectangle
    /// </summary>
    public static float Area(this Rect rect)
    {
        return rect.width * rect.height;
    }

    /// <summary>
    /// Returns the area of this rectangle
    /// </summary>
    public static int Area(this RectInt rect)
    {
        return rect.width * rect.height;
    }

    /// <summary>
    /// Returns a random point from within this rect
    /// </summary>
    public static Vector2 RandomPoint(this Rect rect)
    {
        return new Vector2(
            Random.Range(rect.xMin, rect.xMax),
            Random.Range(rect.yMin, rect.yMax));
    }

    //
    //  Bounds
    //

    /// <summary>
    /// Returns a Rect representing the XZ plane (bottom and top) of this Bounds
    /// </summary>
    public static Rect XZPlane(this Bounds bounds)
    {
        return new Rect(bounds.min.x, bounds.min.z, bounds.size.x, bounds.size.z);
    }


    //
    //  Button
    //

    /// <summary>
    /// Enables interactions with this Button and triggers its "Normal" animation
    /// </summary>
    public static void Enable(this Button button)
    {
        button.interactable = true;
        button.animator.SetTrigger("Normal");
    }

    /// <summary>
    /// Disables interactions with this Button and triggers its "Disabled" animation
    /// </summary>
    public static void Disable(this Button button)
    {
        button.interactable = false;
        button.animator.SetTrigger("Disabled");
    }

    /// <summary>
    /// Enables or disables this Button
    /// </summary>
    public static void SetEnabled(this Button button, bool enable)
    {
        if (enable)
            button.Enable();
        else
            button.Disable();
    }

    //
    //  Color
    //

    /// <summary>
    /// Sets the red component of this color
    /// </summary>
    public static void SetR(this Color color, float r)
    {
        color = new Color(r, color.g, color.b, color.a);
    }

    /// <summary>
    /// Sets the green component of this color
    /// </summary>
    public static void SetG(this Color color, float g)
    {
        color = new Color(color.r, g, color.b, color.a);
    }

    /// <summary>
    /// Sets the blue component of this color
    /// </summary>
    public static void SetB(this Color color, float b)
    {
        color = new Color(color.r, color.g, b, color.a);
    }

    /// <summary>
    /// Sets the alpha component of this color
    /// </summary>
    public static void SetA(this Color color, float a)
    {
        color = new Color(color.r, color.g, color.b, a);
    }

}
