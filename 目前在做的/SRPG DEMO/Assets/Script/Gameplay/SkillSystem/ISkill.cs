using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Skill
{
    public string SkillName;
    public string SkillDes;
    public int[][] SkillRenge;
    public string SkillTargetCamp;

    public SkillStart OnSkillStart;
    public BeforeSkillActive OnBeforeSkillActive;
    public SkillActive OnSkillActive;
    public AfterSkillActive OnAfterSkillActive;
    public SkillEnd OnSkillEnd;
}
[Serializable]
public class SkillTriggerTime
{
    public IfDetermine[] If;
    public TargetDetermine[] Target;
}

#region 技能触发点
public class SkillStart : SkillTriggerTime
{

}

public class BeforeSkillActive : SkillTriggerTime
{

}

public class SkillActive : SkillTriggerTime
{

}

public class AfterSkillActive : SkillTriggerTime
{

}

public class SkillEnd : SkillTriggerTime
{

}
#endregion

[Serializable]

#region 技能行为
public class AddBuffBehavior
{
    public string BuffName;
}

public class RemoveBuffBehavior
{
    public string RemoveBuffName;
}



public class DamageBehavior
{
    public string DamageValue;
}

public class HealBehavior
{
    public string HealValue;
}
#endregion


public class IfDetermine
{
    public string[] IfParameter;
    public TargetDetermine TargetDetermine;
}

public class TargetDetermine
{
    public string SkillTarget;
    public AddBuffBehavior AddBuff;
    public RemoveBuffBehavior RemoveBuff;
    public DamageBehavior Damage;
    public HealBehavior Heal;
}