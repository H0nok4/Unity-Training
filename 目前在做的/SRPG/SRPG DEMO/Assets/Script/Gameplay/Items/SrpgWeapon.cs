using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SrpgWeapon : item
{
    public string weaponName;
    public string weaponDes;
    public int attack;
    public int magicAttack;
    public int hitChance;
    public int[][] attackRenge;
    public int maxUseTimes;
    public string[] buffs;
}
