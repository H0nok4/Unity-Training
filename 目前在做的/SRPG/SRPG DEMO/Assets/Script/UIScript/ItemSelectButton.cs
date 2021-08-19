using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSelectButton : Button
{
    [SerializeField] private SrpgUseableItem m_Item;
    [SerializeField] private Image m_itemImage;
    [SerializeField] private TMP_Text m_ItemNameText;
    public SrpgUseableItem item
    {
        get { return m_Item; }
        private set { m_Item = value; }
    }

    public void SetItem(SrpgUseableItem item)
    {
        m_Item = item;
        m_itemImage.sprite = item.sprite;
        m_ItemNameText.text = item.m_ItemName;
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        //TO DO：进入选择道具的使用对象状态
        BattleManager.instance.HandleItemUse(m_Item);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        //TO DO：显示道具信息
        Debug.Log("Show des");
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        //TO DO:隐藏道具信息
         
    }
}
