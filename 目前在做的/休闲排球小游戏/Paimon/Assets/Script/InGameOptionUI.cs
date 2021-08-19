using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameOptionUI : PaimonUI
{
    public Canvas inGameOptionCanvas;
    public override void ShowThisUI()
    {
        inGameOptionCanvas.gameObject.SetActive(true);
    }

    public override void HideThisUI()
    {
        GameManager.instance.gameState = GameState.playing;
        inGameOptionCanvas.gameObject.SetActive(false);
    }
}
