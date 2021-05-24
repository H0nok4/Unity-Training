using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 烧伤BUFF
public class Burn : Buff
{
    public Burn()
    {
        id = 1;
        durationType = BuffDurationType.limit;
        maxDurationTimes = 5;
        curDurationTimes = maxDurationTimes;
        maxOverlayTimes = 3;
        curOverlayTimes = 1;
        tags = new List<string>() { "dot", "burn" };
        m_buffEffects = new List<BuffEffect>() { new OnTurnEndReduceUnitHealth_10per(this) };
    }
}
#endregion

#region 强壮BUFF
public class Strong : Buff
{
    public Strong()
    {
        id = 2;
        durationType = BuffDurationType.always;
        maxDurationTimes = 1;
        curDurationTimes = maxDurationTimes;
        maxOverlayTimes = 1;
        curOverlayTimes = 1;
        tags = new List<string>() { "StateUp" };
        m_buffEffects = new List<BuffEffect>() { new StateChangeIncreaseAttack_5per(this) };
    }
}
#endregion

#region 眩晕BUFF
public class Stun : Buff
{
    public Stun()
    {
        id = 3;
        durationType = BuffDurationType.limit;
        maxDurationTimes = 1;
        curDurationTimes = maxDurationTimes;
        maxOverlayTimes = 1;
        curOverlayTimes = 1;
        tags = new List<string>() { "Control" };
        m_buffEffects = new List<BuffEffect>() { new SkipThisTurn(this) };
    }
}
#endregion