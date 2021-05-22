using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StartUIState
{
    PressAnyButton,
    Main,
    PlayerOption,
}

public class StartUIManager : MonoBehaviour
{
    public StartUIState state;
    public GameObject pressAnyButtonUI;
    public GameObject mainUI;

    #region 单例
    public static StartUIManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != null)
            {
                Destroy(gameObject);
            }
        }
    }
    #endregion

    public void HandleUpdate()
    {
        if(state == StartUIState.PressAnyButton)
        {
            HandlePlayerClickPressAnyButtonKey();
        }
    }

    public void HandlePlayerClickPressAnyButtonKey()
    {
        if (PlayerInputManager.instance.ClickAnyButton())
        {
            SwitchPressAnyButtonUIToMainUI();
            state = StartUIState.Main;
        }
    }

    public void SwitchPressAnyButtonUIToMainUI()
    {
        pressAnyButtonUI.SetActive(false);
        mainUI.SetActive(true);
    }
}
