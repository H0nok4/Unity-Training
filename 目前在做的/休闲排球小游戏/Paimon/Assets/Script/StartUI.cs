using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUI : PaimonUI
{

    public Canvas startUICanvas;
    public override void ShowThisUI()
    {
        startUICanvas.gameObject.SetActive(true);
    }

    public override void HideThisUI()
    {
        startUICanvas.gameObject.SetActive(false);
    }
}
