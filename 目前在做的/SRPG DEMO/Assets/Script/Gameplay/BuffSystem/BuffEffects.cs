using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 每回合结束时减少 10 * 层数 的血量
public class OnTurnEndReduceUnitHealth_10per : BuffEffect
{
    public OnTurnEndReduceUnitHealth_10per(Buff buff)
    {
        this.m_Buff = buff;
    }

    public override void OnTurnEnd(SrpgClassUnit unit)
    {
        unit.ChangeHealth(m_Buff.curOverlayTimes * 10);
    }
}
#endregion

#region 每层增加5点攻击
public class StateChangeIncreaseAttack_5per : BuffEffect
{
    public StateChangeIncreaseAttack_5per(Buff buff)
    {
        this.m_Buff = buff;
    }

    public override void StateChange(ref int stateValue, SrpgClassPropertyType type)
    {
        if(type == SrpgClassPropertyType.Attack)
        {
            stateValue += 5 * m_Buff.curOverlayTimes;
        }
    }
}
#endregion

#region 每层增加10点防御
public class StateChangeIncreaseDefense_10per : BuffEffect
{
    public StateChangeIncreaseDefense_10per(Buff buff)
    {
        m_Buff = buff;
    }

    public override void StateChange(ref int stateValue, SrpgClassPropertyType type)
    {
        if(type == SrpgClassPropertyType.Defense)
        {
            stateValue += 10 * m_Buff.curOverlayTimes;
        }
    }
}

#endregion

#region 眩晕
public class SkipThisTurn : BuffEffect
{
    public SkipThisTurn(Buff buff)
    {
        m_Buff = buff;
    }

    public override void OnTurnStart(SrpgClassUnit unit)
    {
        BattleManager.instance.SetUnitActived(unit);
    }
}

#endregion