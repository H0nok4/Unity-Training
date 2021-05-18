using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelectPanel : MonoBehaviour
{
    [SerializeField] ItemSelectButton[] itemButtons;
    
    public void InitItemButtons(SrpgClass curClass)
    {
        Debug.Log(curClass.items.Count);
        for(int i = 0; i < curClass.items.Count; i++)
        {
            itemButtons[i].gameObject.SetActive(true);
            itemButtons[i].SetItem(curClass.items[i]);
        }

        for(int i = curClass.items.Count; i < itemButtons.Length; i++)
        {
            itemButtons[i].gameObject.SetActive(false);
        }
    }
}
