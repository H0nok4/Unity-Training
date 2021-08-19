using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackButton : Button
{
    
    public override void OnPointerClick(PointerEventData eventData)
    {
        BattleManager.instance.Action_Attack();
    }

}
