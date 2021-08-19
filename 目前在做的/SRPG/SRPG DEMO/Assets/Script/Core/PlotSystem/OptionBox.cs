using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionBox : MonoBehaviour
{
    [SerializeField] GameObject optionButtonGameObject;
    private List<OptionButton> optionButtons = new List<OptionButton>();
    public void InitOptionButton(string OptionVar,string OptionText)
    {
        var optionObject = Instantiate(optionButtonGameObject, this.transform);
        OptionButton optionButton = optionObject.GetComponent<OptionButton>();
        optionButton.SetOptionButton(OptionVar, OptionText);
        optionButtons.Add(optionButton);
    }

    public void ClearAllOptionButton()
    {
        foreach(var opB in optionButtons)
        {
            Destroy(opB.gameObject);
        }

        optionButtons.Clear();
    }

    public void Display(bool m_Active)
    {
        gameObject.SetActive(m_Active);
    }
}
