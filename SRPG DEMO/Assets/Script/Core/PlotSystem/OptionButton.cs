using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class OptionButton : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] TMP_Text optionText;
    [SerializeField] string curOptionVar;

    public void SetOptionButton(string optionVar,string optionText)
    {
        curOptionVar = optionVar;
        this.optionText.text = optionText;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        GameDirecter.instance.HandlePlayerSelectOption(this.curOptionVar);
    }
}
