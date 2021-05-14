using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUIPanel : UIBase
{
    [SerializeField] SubTextUIWindow top_SubTextUIWindow;
    [SerializeField] SubTextUIWindow mid_SubTextUIWindow;
    [SerializeField] SubTextUIWindow down_SubTextUIWindow;

    public bool isWriting
    {
        get { return top_SubTextUIWindow.isWriting || mid_SubTextUIWindow.isWriting || mid_SubTextUIWindow.isWriting; }
    }

    public SubTextUIWindow GetWritingTextUI()
    {
        if (top_SubTextUIWindow.isWriting)
        {
            return top_SubTextUIWindow;
        }

        if (mid_SubTextUIWindow.isWriting)
        {
            return mid_SubTextUIWindow;
        }

        if (down_SubTextUIWindow.isWriting)
        {
            return down_SubTextUIWindow;
        }

        return null;
        
    }

    public void WriteTextImmediately()
    {
        var TextUI = GetWritingTextUI();

        if(TextUI == null)
        {
            return;
        }

        TextUI.WriteText();
    }

    public void WriteText(string text,string position,bool async)
    {
        SubTextUIWindow textWindow;
        switch (position)
        {
            case "top":
                mid_SubTextUIWindow.Display(false);
                textWindow = top_SubTextUIWindow;
                break;
            case "down":
                mid_SubTextUIWindow.Display(false);
                textWindow = down_SubTextUIWindow;
                break;
            default:
                top_SubTextUIWindow.Display(false);
                down_SubTextUIWindow.Display(false);
                textWindow = mid_SubTextUIWindow;
                break;

        }

        textWindow.Display(true);
        if (async)
        {   
            textWindow.WriteTextAsync(text);
        }
        else
        {
            textWindow.WriteText(text);
        }


    }

    public void HideIcon()
    {
        mid_SubTextUIWindow.DisplayIcon(false);
        top_SubTextUIWindow.DisplayIcon(false);
        down_SubTextUIWindow.DisplayIcon(false);
    }
}
