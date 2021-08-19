using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#region 小药水
public class SmallPotion : SrpgUseableItem
{
    public SmallPotion()
    {
        sprite = ResourceManager.itemSpriteDic["SmallPotion"];
        m_ItemName = "SmallPotion";
        m_ItemUseTarget = UseTarget.self;
        m_UseRenge = new int[1][]{
                    new int[1]{ 1 }
                    };
        m_Des = "Can healer user 35% maxHP";
    }


    public override void Execute(SrpgClassUnit srpgClass)
    {

        if(srpgClass.CurHealth / (float)srpgClass.maxHealth <= 0.65f)
        {
            srpgClass.ChangeHealth(-(int)(srpgClass.maxHealth * 0.35f));
        }
        else
        {
            srpgClass.ChangeHealth(-srpgClass.maxHealth);
        }
    }
}
#endregion

#region 绷带
public class Bandage : SrpgUseableItem
{
    public Bandage()
    {
        sprite = ResourceManager.itemSpriteDic["Bandage"];
        m_ItemName = "Bandage";
        m_ItemUseTarget = UseTarget.ally;
        m_UseRenge = new int[3][]
        {
            new int[3]{ 0,1,0 },
            new int[3]{ 1,0,1 },
            new int[3]{ 0,1,0 }
        };
        m_Des = "Can healer target 25% maxHP";
    }

    public override void Execute(SrpgClassUnit srpgClass)
    {

        if (srpgClass.CurHealth / (float)srpgClass.maxHealth <= 0.75f)
        {
            srpgClass.ChangeHealth(-(int)(srpgClass.maxHealth * 0.25f));
        }
        else
        {
            srpgClass.ChangeHealth(-srpgClass.maxHealth);
        }
    }
}

#endregion

#region 炸弹
public class Bomb : SrpgUseableItem
{
    public Bomb()
    {
        sprite = ResourceManager.itemSpriteDic["Bomb"];
        m_ItemName = "Bomb";
        m_ItemUseTarget = UseTarget.enemy;
        m_UseRenge = new int[5][]
        {
            new int[5]{ 0,0,1,0,0 },
            new int[5]{ 0,0,1,0,0 },
            new int[5]{ 1,1,0,1,1 },
            new int[5]{ 0,0,1,0,0 },
            new int[5]{ 0,0,1,0,0 },
        };
        m_Des = "Can give target 20~50 damage";
    }



    public override void Execute(SrpgClassUnit srpgClass)
    {
        int damage = UnityEngine.Random.Range(20, 50);
        srpgClass.ChangeHealth(damage);
    }
}
#endregion
