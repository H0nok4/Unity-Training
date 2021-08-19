using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverUI : PaimonUI
{
    public Canvas gameOverCanvas;
    public TMP_Text winText;
    public TMP_Text loseText;
    public override void ShowThisUI()
    {
        gameOverCanvas.gameObject.SetActive(true);
        if(GameManager.instance.Player1Score >= 15)
        {
            winText.gameObject.SetActive(true);
        }
        else
        {
            loseText.gameObject.SetActive(true);
        }
    }

    public override void HideThisUI()
    {
        winText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(false);
    }
}
