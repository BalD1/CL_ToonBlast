using UnityEngine;

public static class GameSettings
{
#if UNITY_EDITOR
    public const bool UnityEditor = true;
#else
    public const bool UnityEditor = false;
#endif
}