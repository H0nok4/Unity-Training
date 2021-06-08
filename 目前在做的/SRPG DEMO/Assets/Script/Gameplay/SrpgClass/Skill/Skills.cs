using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Slap : SkillBase
{
    public Slap()
    {
        skillName = "Slap";
        skillDes = "Use weapon Slap target, damage target by 120% Attack";
        skillTarget = UseTarget.enemy;
        skillRenge = new int[3][] { new int[] {0,1,0},
                                    new int[] {1,0,1},
                                    new int[] {0,1,0},};
    }

    public override void Execute(SrpgClassUnit self, SrpgClassUnit target)
    {
        //TO DO:技能效果
        target.OnDamaged(self,(int)(self.attack * 1.2f),true);
        int chance = 100;
        int point = Random.Range(0, 101);
        if(point <= chance)
        {
            target.buffManager.AddBuff("Stun");
        }
    }
}