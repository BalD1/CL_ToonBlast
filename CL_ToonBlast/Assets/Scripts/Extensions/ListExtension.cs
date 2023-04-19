using System.Collections.Generic;
using UnityEngine;

public static class ListExtension
{
    public static int RandomIndex<T>(this List<T> array) => Random.Range(0, array.Count);

    public static T RandomElement<T>(this List<T> array)
    {
        if (array.Count <= 0) return default(T);
        return array[Random.Range(0, array.Count)];
    }
}