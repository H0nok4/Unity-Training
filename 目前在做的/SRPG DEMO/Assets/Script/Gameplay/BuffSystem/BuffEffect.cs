using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEffect : IBuffEffect
{
    public Buff m_Buff;
    public virtual void StateChange(ref int stateValue, SrpgClassPropertyType type)//√在SrpgClassUnit的GetState方法中触发
    {

    }

    public virtual void OnAttack(SrpgClassUnit unit, SrpgClassUnit target,DamageDetail curDamageDetail,DamageDetail OriginalDamageDetail)//√在SrpgClassUnit的被攻击方法中触发
    {
        
    }

    public virtual void OnDefend(SrpgClassUnit unit, SrpgClassUnit attacker,DamageDetail curDamageDetail,DamageDetail OriginalDamageDetail)//√在SrpgClassUnit的被攻击方法中触发
    {
        
    }

    public virtual void OnTurnEnd(SrpgClassUnit unit)//√在BattleManager的SetUnitActived方法中触发
    {
        
    }

    public virtual void OnTurnStart(SrpgClassUnit unit)//√在BattleManager的StartNewTurn方法中触发
    {
        
    }

    public virtual void OnBuffAdd()
    {

    }

    public virtual void OnBuffRemove()
    {

    }


}
