using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#region 小药水
public class SmallPotion : SrpgUseableItem
{
    public new Sprite sprite = Resources.Load<Sprite>("Sprite/ItemSprite/Potion");
    public new string m_ItemName = "SmallPotion";
    public new ItemUseTarget m_ItemUseTarget = ItemUseTarget.self;
    public new int[][] m_UseRenge = new int[1][]
    {
        new int[1]{ 1 }
    };
    public new string m_Des = "Can healer user 35% maxHP";

    public override void Execute(SrpgClass srpgClass)
    {

        if(srpgClass.CurHealth / (float)srpgClass.classProperty[SrpgClassPropertyType.MaxHealth] <= 0.65f)
        {
            srpgClass.CurHealth += (int)(srpgClass.classProperty[SrpgClassPropertyType.MaxHealth] * 0.35f);
        }
        else
        {
            srpgClass.CurHealth = srpgClass.classProperty[SrpgClassPropertyType.MaxHealth];
        }
    }
}
#endregion

#region 绷带
public class Bandage : SrpgUseableItem
{
    public new string m_ItemName = "Bandage";
    public new ItemUseTarget m_ItemUseTarget = ItemUseTarget.ally;
    public new int[][] m_UseRenge = new int[3][]
    {
        new int[3]{ 0,1,0 },
        new int[3]{ 1,0,1 },
        new int[3]{ 0,1,0 }
    };
    public new string m_Des = "Can healer target 25% maxHP";

    public override void Execute(SrpgClass srpgClass)
    {

        if (srpgClass.CurHealth / (float)srpgClass.classProperty[SrpgClassPropertyType.MaxHealth] <= 0.75f)
        {
            srpgClass.CurHealth += (int)(srpgClass.classProperty[SrpgClassPropertyType.MaxHealth] * 0.25f);
        }
        else
        {
            srpgClass.CurHealth = srpgClass.classProperty[SrpgClassPropertyType.MaxHealth];
        }
    }
}

#endregion

#region 炸弹
public class Bomb : SrpgUseableItem
{

    public new string m_ItemName = "Bomb";
    public new ItemUseTarget m_ItemUseTarget = ItemUseTarget.ally;
    public new int[][] m_UseRenge = new int[5][]
    {
        new int[5]{ 0,0,1,0,0 },
        new int[5]{ 0,0,1,0,0 },
        new int[5]{ 1,1,0,1,1 },
        new int[5]{ 0,0,1,0,0 },
        new int[5]{ 0,0,1,0,0 },
    };
    public new string m_Des = "Can give target 20~50 damage";

    public override void Execute(SrpgClass srpgClass)
    {
        int damage = UnityEngine.Random.Range(20, 50);
        

    }
}
#endregion
