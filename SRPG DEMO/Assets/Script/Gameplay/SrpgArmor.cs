using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SrpgArmor : item
{
    public SrpgArmor()
    {

    }
    public SrpgArmor(SrpgArmor armorBase)
    {
        this.armorName = armorBase.armorName;
        this.armorDes = armorBase.armorDes;
        this.health = armorBase.health;
        this.defense = armorBase.defense;
        this.avoid = armorBase.defense;

    }
    //TO DO:护甲有名字，描述，防御力，闪避率
    public string armorName;
    public string armorDes;
    public int health;
    public int defense;
    public int magicDefense;
    public int avoid;


}

