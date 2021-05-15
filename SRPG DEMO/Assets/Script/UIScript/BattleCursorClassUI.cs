using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleCursorClassUI : MonoBehaviour
{
    public GameObject UIPanel;
    public Image classImage;
    public TMP_Text className;
    public Slider classHPSlider;
    public TMP_Text classHPText;

    public void UpdateUI(SrpgClass srpgClass)
    {
        if(srpgClass != null)
        {
            if (UIPanel.activeSelf != true)
                UIPanel.SetActive(true);

            //TO DO:添加Class头像 classImage =
            className.text = srpgClass.classType.ToString();
            classHPSlider.maxValue = srpgClass.classProperty[SrpgClassPropertyType.MaxHealth];
            classHPSlider.value = srpgClass.CurHealth;
            classHPText.text = srpgClass.CurHealth.ToString();
        }
        else
        {
            UIPanel.SetActive(false);
        }
    }

}
