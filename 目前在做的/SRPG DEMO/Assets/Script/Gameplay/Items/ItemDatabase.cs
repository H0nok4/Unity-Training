using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ItemDatabase
{
    public static void Init()
    {
        foreach (var KeyValuePair in weapon_Dictionary)
        {
            var weapon_Name = KeyValuePair.Key;
            var weapon = KeyValuePair.Value;

            weapon.weaponName = weapon_Name;
        }

        foreach (var KeyValuePair in armor_Dictionary)
        {
            var armor_Name = KeyValuePair.Key;
            var armor = KeyValuePair.Value;

            armor.armorName = armor_Name;
        }

        foreach(var kvp in Items_Dictionary)
        {
            var itemName = kvp.Key;
            var item = kvp.Value;

            item.m_ItemName = itemName;
        }

        //序列化Test
        string jsonPath = "Assets/WeaponData/WeaponDataBase.json";

        File.WriteAllText(jsonPath, JsonConvert.SerializeObject(weapon_Dictionary));
    }

    public static Dictionary<string, SrpgWeapon> weapon_Dictionary { get; set; } = new Dictionary<string, SrpgWeapon>()
    {
        {
            "IronSword",
            new SrpgWeapon()
            {
                //TO DO:添加sprite
                weaponName = "IronSword",
                weaponDes = "a sword is made of iron",
                attack = 7,
                magicAttack = 0,
                hitChance = 95,
                attackRenge = new int[3][]{ new int[3] {0,1,0},new int[3] {1,0,1},new int[3] {0,1,0} },
                maxUseTimes = 15,
                onDamageTarget = (SrpgClassUnit srpgClass) =>
                {
                    //附加固定伤害
                    srpgClass.CurHealth -= 4;
                    if(srpgClass.CurHealth <= 0)
                    {
                        srpgClass.OnDispawn();
                    }
                    Debug.Log("more attack 4");
                },
                onDamageSelf = (SrpgClassUnit srpgClass) =>
                {
                    //吸血2滴血
                    if(srpgClass.CurHealth + 2 < srpgClass.maxHealth)
                    {
                        srpgClass.CurHealth += 2;
                    }
                    else
                    {
                        srpgClass.CurHealth = srpgClass.maxHealth;
                    }
                }
                
            }
        },
        {
            "IronSword + 1",
            new SrpgWeapon()
            {
                weaponName = "IronSword +1",
                weaponDes = "an sword is made of iron",
                attack = 9,
                magicAttack = 0,
                hitChance = 100,
                attackRenge = new int[3][]{ new int[3] {0,1,0},new int[3] {1,0,1},new int[3] {0,1,0} },
                maxUseTimes = 15
            }
        },
    };

    public static Dictionary<string, SrpgArmor> armor_Dictionary { get; set; } = new Dictionary<string, SrpgArmor>()
    {
        {
            "Leather",
            new SrpgArmor()
            {
                armorName = "Leather",
                armorDes = "a armor made of leather",
                health = 10,
                defense = 15,
                magicDefense = 0,
                avoid = 20
            }
        }
    };

    public static Dictionary<string, SrpgUseableItem> Items_Dictionary { get; set; } = new Dictionary<string, SrpgUseableItem>()
    {
        {
            "SmallPotion",
            new SmallPotion()
        },

        { 
            "Bandage",
            new Bandage()
        },

        {
            "Bomb",
            new Bomb()
        },
    };

}

public class DataInfor
{
    public string name;
    public Dictionary<string, SrpgWeapon> weaponDataBase;
}
