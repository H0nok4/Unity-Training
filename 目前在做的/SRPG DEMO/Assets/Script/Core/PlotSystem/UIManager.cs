using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] DialogBox upDialogBox;
    [SerializeField] DialogBox midDialogBox;
    [SerializeField] DialogBox downDialogBox;

    [SerializeField] OptionBox optionBox;
    public static UIManager instance;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
    public bool isWriting
    {
        get { return upDialogBox.isWriting || midDialogBox.isWriting || downDialogBox.isWriting; }
    }

    public void ClearAll()
    {
        downDialogBox.Display(false);
        midDialogBox.Display(false);
        midDialogBox.Display(false);
        optionBox.Display(false);
    }

    public DialogBox GetWritingDialogBox()
    {
        if (upDialogBox.isWriting)
        {
            return upDialogBox;
        }
        if (midDialogBox.isWriting)
        {
            return midDialogBox;
        }
        if (downDialogBox.isWriting)
        {
            return downDialogBox;
        }

        return null;
    }

    public void WritingText(string position,string textName,string text,bool async)
    {
        DialogBox dialogBox;
        switch (position)
        {
            case "top":
                dialogBox = upDialogBox;
                midDialogBox.Display(false);
                break;
            case "mid":
                dialogBox = midDialogBox;
                upDialogBox.Display(false);
                downDialogBox.Display(false);
                break;
            default:
                dialogBox = downDialogBox;
                midDialogBox.Display(false);
                break;
                
        }

        dialogBox.Display(true);
        if (async)
        {
            dialogBox.WriteTextAsync(text,textName);
        }
        else
        {
            dialogBox.WriteText(text, textName);
        }

    }

    public void WriteTextImmediately()
    {
        GetWritingDialogBox().WriteText();
    }

    public void SetOptionBox(string optionVar,string optionText)
    {
        if (optionBox.gameObject.activeSelf == false)
            optionBox.Display(true);

        optionBox.InitOptionButton(optionVar,optionText);
    }

    public void ClearOptions()
    {
        optionBox.ClearAllOptionButton();
        optionBox.Display(false);
    }
}
