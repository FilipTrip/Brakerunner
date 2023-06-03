using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public static class Extensions
{
    private static readonly Regex regex = new Regex("([A-Z]+(?=$|[A-Z][a-z])|[A-Z]?[a-z0-9]+)", RegexOptions.Compiled);

    //
    //  String
    //

    /// <summary>
    /// Splits a PascalCase string using Regex. Splits numbers as well.
    /// </summary>
    public static string SplitPascalCase(this string value)
    {
        return regex.Replace(value, " $1").Trim();
    }

    //
    //  Math
    //

    /// <summary>
    /// Returns a new float to be used, instead of this float, as argument t in any lerp function.
    /// Perfect accuracy. Subtle effect. See plotted function: https://www.desmos.com/calculator/ktcwf5obja
    /// </summary>
    public static float Smoothstep(this float x)
    {
        // Classic smoothstep function

        x = Mathf.Clamp01(x);
        return x * x * (3.0f - 2.0f * x);
    }

    /// <summary>
    /// Returns a new float to be used, instead of this float, as argument t in any lerp function.
    /// Perfect accuracy. Customizable effect. See plotted function: https://www.desmos.com/calculator/ktcwf5obja
    /// </summary>
    /// <param name="x">Input value (0 to 1)</param>
    /// <param name="a">Function amplitude (-0.98 to 0.98)</param>
    /// <param name="p">Position of function separator (0 to 1)</param>
    public static float CustomSmoothstep(this float x, float a = 0.5f, float p = 0.5f)
    {
        if (a == 0f)
            return Mathf.Clamp01(x);

        x = Mathf.Clamp01(x);
        a = Mathf.Clamp(a, -0.98f, 0.98f);
        p = Mathf.Clamp01(p);

        float c = 2 / (1 - a) - 1; // Function amplitude

        float F(float x, float n) // Function
        {
            return Mathf.Pow(x, c) / Mathf.Pow(n, c - 1);
        }

        return x < p ? F(x, p) : 1 - F(1 - x, 1 - p); // Output
    }

    /// <summary>
    /// Remaps this value from one range to another.
    /// </summary>
    public static void Remap(this ref float x, float min1, float max1, float min2, float max2)
    {
        x = (x - min1) / (max1 - min1) * (max2 - min2) + min2;
    }

    /// <summary>
    /// Returns this value remapped from one range to another.
    /// </summary>
    public static float Remapped(this float x, float min1, float max1, float min2, float max2)
    {
        return (x - min1) / (max1 - min1) * (max2 - min2) + min2;
    }

    /// <summary>
    /// Wraps this value to the provided range.
    /// </summary>
    public static void Wrap(this ref float x, float min, float max)
    {
        x = x - (max - min) * Mathf.Floor(x / (max - min));
    }

    /// <summary>
    /// Returns this value wrapped to the provided range.
    /// </summary>
    public static float Wrapped(this float x, float min, float max)
    {
        return x - (max - min) * Mathf.Floor(x / (max - min));
    }

    //
    //  Enum
    //

    /// <summary>
    /// Returns the next value of this enum.
    /// </summary>
    public static T GetNext<T>(this T value) where T : struct
    {
        if (!typeof(T).IsEnum)
            throw new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));

        T[] array = (T[])Enum.GetValues(value.GetType());
        int i = Array.IndexOf(array, value) + 1;

        return (i == array.Length) ? array[0] : array[i];
    }

    /// <summary>
    /// Returns the previous value of this enum.
    /// </summary>
    public static T GetPrevious<T>(this T value) where T : struct
    {
        if (!typeof(T).IsEnum)
            throw new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));

        T[] array = (T[])Enum.GetValues(value.GetType());
        int i = Array.IndexOf(array, value) - 1;

        return (i == -1) ? array[array.Length - 1] : array[i];
    }

    /// <summary>
    /// Sets the value of this enum to the next value and returns it.
    /// </summary>
    public static T SetNext<T>(this ref T value) where T : struct
    {
        return value = value.GetNext();
    }

    /// <summary>
    /// Sets the value of this enum to the previous value and returns it.
    /// </summary>
    public static T SetPrevious<T>(this ref T value) where T : struct
    {
        return value = value.GetPrevious();
    }

    //
    //  List
    //

    /// <summary>
    /// Shuffles this list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> values)
    {
        for (int i = 0; i < values.Count; ++i)
        {
            T temp = values[i];
            int randomIndex = Random.Range(i, values.Count);
            values[i] = values[randomIndex];
            values[randomIndex] = temp;
        }
    }

    /// <summary>
    /// Returns a random item from the list.
    /// </summary>
    public static T GetRandom<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    /// <summary>
    /// Removes and returns a random item from the list.
    /// </summary>
    public static T RemoveRandom<T>(this List<T> list)
    {
        int i = Random.Range(0, list.Count);
        T item = list[i];
        list.RemoveAt(i);
        return item;
    }

    /// <summary>
    /// Adds the content of the provided List to this List
    /// </summary>
    public static List<T> Add<T>(this List<T> list, List<T> other)
    {
        foreach (T item in other)
            list.Add(item);

        return list;
    }

    /// <summary>
    /// Returns a new List containing the content of this and the provided List
    /// </summary>
    public static List<T> And<T>(this List<T> list, List<T> other)
    {
        List<T> joinedList = new List<T>(list);

        foreach (T item in other)
            joinedList.Add(item);

        return joinedList;
    }

}
