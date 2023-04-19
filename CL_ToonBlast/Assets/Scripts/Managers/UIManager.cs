using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    private void Start()
    {
        GameManager.Instance.D_gameStateChange += OnStateChange;
    }

    public void OnStateChange(GameManager.E_GameStates newState)
    {
        switch (newState)
        {
            default:
                break;
        }
    }
}
