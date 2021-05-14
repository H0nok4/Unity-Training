using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SrpgClassPropertyType
{
    Attack,Defense,MaxHealth,MagicAttack,MagicDefense,Avoid,CritChance,HitChanceBase,CritDamage
}

[CreateAssetMenu(fileName = "New ClassProperty.asset",menuName = "SPRG/ClassProperty")]
public class SrpgClassProperty : ScriptableObject
{
    public int attack;
    public int defense;
    public int maxHealth;
    public int magicAttack;
    public int magicDefense;
    public int avoid;
    public int critChance;
    public int critDamage;
    public int hitChanceBase;

    //TO DO:升级奖励
}
