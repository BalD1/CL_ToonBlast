#if UNITY_EDITOR
using System;
using UnityEditor;
 using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class CustomBuildProcessor : IPreprocessBuildWithReport
{ 
    public int callbackOrder { get { return 0; } }

    public static event Action OnPreBuild = () => { };

    public void OnPreprocessBuild(BuildReport report)
    {
        OnPreBuild?.Invoke();
    }
}
 #endif