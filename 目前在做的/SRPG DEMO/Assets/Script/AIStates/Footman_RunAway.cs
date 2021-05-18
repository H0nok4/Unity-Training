using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footman_RunAway : AIState
{
    /* RunAway状态下，AI会优先保住自己的性命
     * Execute方法具体逻辑：先判断身上的道具是否有血瓶等能恢复的道具，如果有
     * ReciveMessager方法具体逻辑:当接收到消息"Msg_HelperMe!"的时候，将当前状态改变为帮助目标状态
     */
    private static Footman_RunAway instance;
    public static Footman_RunAway Instance()
    {
        if (instance == null) instance = new Footman_RunAway();
        return instance;
    }


    public override void Enter(SrpgClass srpgClass)
    {

    }

    public override IEnumerator Execute(SrpgClass srpgClass)
    {
        srpgClass.isRunningAI = true;
        var moveRenge = pathFinder.CreatAIMoveRenge(srpgClass);


        if(srpgClass.CurHealth / (float)srpgClass.classProperty[SrpgClassPropertyType.MaxHealth] >= 0.4)
        {
            //TO DO：脱离危险，继续打击敌人
            srpgClass.StateMeching.ChangeCurState(Footman_Attack.Instance());
        }
        yield break;
        srpgClass.IsActived = true;
        srpgClass.isRunningAI = false;
    }

    public override void Exit(SrpgClass srpgClass)
    {

    }

    public override void ReciveMessager(Message message)
    {

    }

    public override void SendMessage()
    {

    }

}