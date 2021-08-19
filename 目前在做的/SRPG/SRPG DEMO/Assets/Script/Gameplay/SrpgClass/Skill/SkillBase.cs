using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillBase
{
    void Execute(SrpgClassUnit self, SrpgClassUnit target);
}

//该技能系统已经弃用
//新的技能系统为数据驱动型的，在SkillSystem中
public class SkillBase : ISkillBase
{
    public string skillName;
    public string skillDes;
    public UseTarget skillTarget;
    public int[][] skillRenge;

    public virtual void Execute(SrpgClassUnit self, SrpgClassUnit target)
    {
        
    }

    public virtual void PlaySound()
    {

    }

    public virtual void PlayParticle()
    {

    }
}