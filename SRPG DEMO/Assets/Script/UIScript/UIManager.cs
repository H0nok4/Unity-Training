using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    Stack<UIBase> uiStack;
    Dictionary<string, UIBase> uiDict;
    [SerializeField] Canvas canvasObject;

    private void Awake()
    {
        uiStack = new Stack<UIBase>();
    }

    public void CloseAllUI()
    {
        while(uiStack.Count> 0)
        {
            var ui = uiStack.Pop();
            ui.CloseUI();
            //TO DO:清空UI，然后设置Active为False
        }
    }

}
