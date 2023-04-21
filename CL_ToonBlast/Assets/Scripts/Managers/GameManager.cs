using System;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public enum E_ScenesNames
    {
        MainScene,
    }

#if UNITY_EDITOR
    public static Action<bool> UnityEditorFocusChanged
    {
        get
        {
            var fieldInfo = typeof(EditorApplication).GetField("focusChanged",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            return (Action<bool>)fieldInfo.GetValue(null);
        }
        set
        {
            var fieldInfo = typeof(EditorApplication).GetField("focusChanged",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            fieldInfo.SetValue(null, value);
        }
    } 
#endif

    #region GameState

    public enum E_GameStates
    {
    }

    private E_GameStates gameState;
    public E_GameStates GameState
    {
        get => gameState;
        set
        {
            gameState = value;

            D_gameStateChange?.Invoke(value);
        }
    }

    public delegate void D_GameStateChange(E_GameStates newState);
    public D_GameStateChange D_gameStateChange;

    #endregion

    protected override void Awake()
    {
        base.Awake();

    }

    #region Scene

    public static bool CompareCurrentScene(E_ScenesNames nameToCompare) => CompareCurrentScene(nameToCompare.ToString());
    public static bool CompareCurrentScene(string nameToCompare) => SceneManager.GetActiveScene().name == nameToCompare;

    public static void LoadScene(E_ScenesNames sceneToLoad) => LoadScene(sceneToLoad);
    public static void LoadScene(string sceneToLoad)
    {
        if (CompareCurrentScene(sceneToLoad)) return;

        SceneManager.LoadScene(sceneToLoad.ToString());
    }

    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    } 

    #endregion
}
