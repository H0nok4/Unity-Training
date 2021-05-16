﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemButton : Button
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        GameObject.Find("GameManager").GetComponent<BattleManager>().Action_Item();
    }
}