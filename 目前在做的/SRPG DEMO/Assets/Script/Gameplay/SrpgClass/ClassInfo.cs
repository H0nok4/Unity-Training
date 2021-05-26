using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SrpgClassPropertyType
{
    Attack, Defense, MaxHealth, MagicAttack, MagicDefense, Avoid, CritChance, HitChanceBase, CritDamage
}

[CreateAssetMenu(fileName = "New ClassInfo.asset", menuName = "SRPG/ClassInfo")]
public class ClassInfo : ScriptableObject
{
    public ClassType classType;
    public int classMovePoint;
    public float m_MoveSpeed;

    public List<Sprite> walkDownSprites;
    public List<Sprite> walkUpSprites;
    public List<Sprite> walkLeftSprites;
    public List<Sprite> walkRightSprites;
    //TO DO:升级所需的经验值
    public int attack;
    public int defense;
    public int maxHealth;
    public int magicAttack;
    public int magicDefense;
    public int avoid;
    public int critChance;
    public int critDamage;
    public int hitChanceBase;

    //TO DO:每次升级的增加的所需经验值
    public int attackLevelBonus;
    public int defenseLevelBonus;
    public int maxHealthLevelBonus;
    public int magicAttackBonus;
    public int magicDefenseBonus;
}
