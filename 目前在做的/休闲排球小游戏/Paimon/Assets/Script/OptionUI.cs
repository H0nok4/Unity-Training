using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionUI : PaimonUI
{
    public Canvas optionUICanvas;
    public override void ShowThisUI()
    {
        optionUICanvas.gameObject.SetActive(true);
    }

    public override void HideThisUI()
    {
        optionUICanvas.gameObject.SetActive(false);
    }
}
