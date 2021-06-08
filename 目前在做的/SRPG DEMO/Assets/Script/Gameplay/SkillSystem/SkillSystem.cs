using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum SkillSystemError
{
    NoneError,TriggerNoneDetermineError,IfDetermineParameterError,IfDetermineParameterChanceIsNotValidError,
    TargetDetermineSkillTargetError,DamageBehaviorParameterError,AddBuffBehaviorBuffNameError, RemoveBuffBehaviorBuffNameError, HealBehaviorParameterError
}
public class SkillSystem
{
    #region 介绍
    /*  这个系统是将Skill分为五个触发点，四种行为，俩种判断方法的数据驱动型技能，可以解析技能Json文件,变成Skill类，然后传进来实现效果
     *  首先，技能有"开始" "生效前" "生效时" "生效后" "结束"五个触发点，在每个触发点可以定义不同的判断条件
     *  判断条件有俩种：
     *  一种是IF，有个Parameter参数作为条件，当条件长度为2的时候，我定义成"Chance 数值%"，等同于有多少概率触发。当长度为3时，我定义为比较，可以通过比较角色的各种属性来触发行为，另外还附带一个Target条件
     *  一种是Target，Target作为判断条件其实我还觉得有点牵强，不过为了这个系统的简单，我还是把他定义为条件，TargetDetermine有个SkillTarget参数，为"Target"时就是鼠标所点击的目标,为Self时就是释放技能的人
     *  为什么会使用Target呢，实际上是为了简化行为判断目标，把需要在行为触发的时候来判断的目标，提前到了条件上，所以行为只需要管自己就好，不用去判断目标
     *  而行为目前定义了4种，分别为"添加Buff" "造成伤害" "回复血量" "移除Buff"
     *  行为可以很方便扩展，根据需求来新建或者修改
     *  *******分割线**********
     *  而整个技能系统，其实就是在每个触发点，递归一直找到最后的行为，然后触发行为罢了(行为树？)。
     *  因为经验不足，所以做得很粗糙，请见谅
     */
    #endregion
    public static bool SkillHandler(Skill skill,SrpgClassUnit selfUnit,SrpgClassUnit targetUnit)//技能系统的调用口，传入技能，技能发生者，目标
    {
        SkillSystemError error = SkillSystemError.NoneError;
        //↓顺序触发五个触发点
        if (skill.OnSkillStart != null)
        {
            OnSkillTriggerHandler(skill.OnSkillStart, selfUnit, targetUnit, ref error);
        }
        if (skill.OnBeforeSkillActive != null)
        {
            OnSkillTriggerHandler(skill.OnBeforeSkillActive, selfUnit, targetUnit, ref error);
        }
        if (skill.OnSkillActive != null)
        {
            OnSkillTriggerHandler(skill.OnSkillActive, selfUnit, targetUnit, ref error);
        }
        if (skill.OnAfterSkillActive != null)
        {
            OnSkillTriggerHandler(skill.OnAfterSkillActive, selfUnit, targetUnit, ref error);
        }
        if (skill.OnSkillEnd != null)
        {
            OnSkillTriggerHandler(skill.OnSkillEnd, selfUnit, targetUnit, ref error);
        }


        if (error != SkillSystemError.NoneError){
            Debug.LogError(error);
            return false;
        }

            return true;
    }

    //技能触发点
    private static bool OnSkillTriggerHandler(SkillTriggerTime onSkillStart,SrpgClassUnit selfUnit,SrpgClassUnit targetUnit,ref SkillSystemError error)
    {
        if (onSkillStart.If != null)
        {
            //顺序触发If判断条件
            foreach(var ifD in onSkillStart.If)
            {
                IfDetermineHandler(ifD, selfUnit, targetUnit, ref error);
            }
            return true;
        }else if(onSkillStart.Target != null)
        {
            //顺序触发Target判断条件
            foreach(var taD in onSkillStart.Target)
            {
                bool succ = TargetDetermineHandler(taD, selfUnit, targetUnit, ref error);
            }
            return true;
        }

        error = SkillSystemError.TriggerNoneDetermineError;
        return false;

    }

    private static bool IfDetermineHandler(IfDetermine determine,SrpgClassUnit selfUnit,SrpgClassUnit targetUnit,ref SkillSystemError error)
    {
        if (determine.IfParameter.Length == 2)
        {
            //如果是2个参数，则是[Chance 百分比数值] 
            if (determine.IfParameter[0] == "Chance")
            {
                int point = UnityEngine.Random.Range(0, 101);
                int chance = 0;
                bool parseSuccessful = int.TryParse(determine.IfParameter[1], out chance);
                if (parseSuccessful == false)
                {
                    error = SkillSystemError.IfDetermineParameterChanceIsNotValidError;
                    return false;
                }

                if (point <= chance)
                {
                    TargetDetermineHandler(determine.TargetDetermine, selfUnit, targetUnit, ref error);
                    return true;
                }

            }
            else
            {
                error = SkillSystemError.IfDetermineParameterError;
                return false;
            }
        }
        else if (determine.IfParameter.Length == 3)
        {
            //TO DO:技能的If条件中添加各种条件
            //Temp:因为需要添加的条件太多，需要针对需求来添加，所以这里暂时先空着，无条件判断正确
            TargetDetermineHandler(determine.TargetDetermine, selfUnit, targetUnit, ref error);
            return true;
        }

        error = SkillSystemError.IfDetermineParameterError;
        return false;
    }

    private static bool TargetDetermineHandler(TargetDetermine determine, SrpgClassUnit selfUnit, SrpgClassUnit targetUnit, ref SkillSystemError error)
    {
        if(determine.SkillTarget == "Target")
        {
            //根据目标触发所有可能的行为
            bool isSucc = true;//isSucc用来帮助判断哪里出错了，如果有地方出错可以在那个位置中止，error会记录最新的错误
            if (determine.Damage != null && isSucc)
            {
                isSucc = DamageBehaviorHandler(determine.Damage, selfUnit, targetUnit, false, ref error);
            }
            if (determine.AddBuff != null && isSucc)
            {
                isSucc = AddBuffBehaviorHandler(determine.AddBuff, selfUnit, targetUnit, false, ref error);
            }
            if(determine.RemoveBuff != null && isSucc)
            {
                isSucc = RemoveBuffBehaviorHandler(determine.RemoveBuff, selfUnit, targetUnit, false,ref error);
            }
            if(determine.Heal != null && isSucc)
            {
                isSucc = HealBehaviorHandler(determine.Heal, selfUnit, targetUnit, false, ref error);
            }

            return isSucc;

        }else if(determine.SkillTarget == "Self")
        {
            bool isSucc = true;
            if (determine.Damage != null && isSucc)
            {
                isSucc = DamageBehaviorHandler(determine.Damage, selfUnit, targetUnit, true, ref error);
            }
            if (determine.AddBuff != null && isSucc)
            {
                isSucc = AddBuffBehaviorHandler(determine.AddBuff, selfUnit, targetUnit, true, ref error);
            }
            if (determine.RemoveBuff != null && isSucc)
            {
                isSucc = RemoveBuffBehaviorHandler(determine.RemoveBuff, selfUnit, targetUnit, true, ref error);
            }
            if (determine.Heal != null && isSucc)
            {
                isSucc = HealBehaviorHandler(determine.Heal, selfUnit, targetUnit, true, ref error);
            }

            return isSucc;
        }

        error = SkillSystemError.TargetDetermineSkillTargetError;
        return false;
    }

    private static bool DamageBehaviorHandler(DamageBehavior damage,SrpgClassUnit selfUnit,SrpgClassUnit targetUnit,bool isSelf,ref SkillSystemError error)
    {
        int damageValue = 0;
        bool successesful = int.TryParse(damage.DamageValue, out damageValue);
        if (successesful == false)
        {
            error = SkillSystemError.DamageBehaviorParameterError;
            return false;
        }

        if (isSelf)
        {
            selfUnit.OnDamaged(selfUnit, damageValue, false);
        }
        else
        {
            targetUnit.OnDamaged(selfUnit, damageValue, true);
        }

        return true;
    }

    private static bool AddBuffBehaviorHandler(AddBuffBehavior addBuff,SrpgClassUnit selfUnit,SrpgClassUnit targetUnit,bool isSelf,ref SkillSystemError error)
    {
        var buffType = Type.GetType(addBuff.BuffName);
        var buff = buffType.Assembly.CreateInstance(addBuff.BuffName);

        if(buff == null)
        {
            error = SkillSystemError.AddBuffBehaviorBuffNameError;
            return false;
        }
        
        if (isSelf)
        {
            selfUnit.buffManager.AddBuff((Buff)buff);
        }
        else
        {
            targetUnit.buffManager.AddBuff((Buff)buff);
            Debug.Log("Skill Succ AddBuff");
        }

        return true;
    }

    private static bool RemoveBuffBehaviorHandler(RemoveBuffBehavior removeBuff, SrpgClassUnit selfUnit, SrpgClassUnit targetUnit, bool isSelf, ref SkillSystemError error)
    {
        var buffType = Type.GetType(removeBuff.RemoveBuffName);
        var buff = buffType.Assembly.CreateInstance(removeBuff.RemoveBuffName);

        if(buff == null)
        {
            error = SkillSystemError.AddBuffBehaviorBuffNameError;
            return false;
        }

        if (isSelf)
        {
            selfUnit.buffManager.RemoveBuff((Buff)buff);
        }
        else
        {
            targetUnit.buffManager.RemoveBuff((Buff)buff);
        }

        return true;
    }

    private static bool HealBehaviorHandler(HealBehavior heal, SrpgClassUnit selfUnit, SrpgClassUnit targetUnit, bool isSelf, ref SkillSystemError error)
    {
        int healValue = 0;
        bool successesful = int.TryParse(heal.HealValue, out healValue);
        if (successesful == false)
        {
            error = SkillSystemError.HealBehaviorParameterError;
            return false;
        }

        //TO DO:这里应该新建一个治疗方法，但是我现在准备测试一下技能系统，如果你还是看到OnDamage方法的话，就是我忘记改了
        if (isSelf)
        {
            selfUnit.OnDamaged(selfUnit, -healValue, false);
        }
        else
        {
            targetUnit.OnDamaged(selfUnit, -healValue, true);
        }

        return true;
    }
}
