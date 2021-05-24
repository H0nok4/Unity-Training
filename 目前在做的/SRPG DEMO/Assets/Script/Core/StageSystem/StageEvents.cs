using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEvent_EnemyRetreat : StageEvent
{
    public StageEvent_EnemyRetreat()
    {
        TriggerTurn = 3;
    }

    public override bool isTriggerCondition()
    {
        return true;
    }

    public override void Action()
    {
        Debug.Log("Enemy Retreat!");
    }


}