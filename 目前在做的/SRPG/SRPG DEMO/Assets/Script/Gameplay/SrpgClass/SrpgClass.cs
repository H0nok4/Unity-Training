using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SrpgClass
{
    [SerializeField] string m_SrpgClassName;
    [SerializeField] Sprite m_SrpgClassSprite;
    [SerializeField] ClassInfo m_ClassInfo;
    [SerializeField] int m_Level;
    //TO DO:经验值
    [SerializeField] string m_weaponName;
    [SerializeField] string m_armorName;
    [SerializeField] SrpgWeapon m_Weapon;
    [SerializeField] SrpgArmor m_Armor;
    [SerializeField] List<SrpgUseableItem> m_Items;

    #region 属性
    public string srpgClassName
    {
        get { return m_SrpgClassName; }
        set { m_SrpgClassName = value; }
    }

    public Sprite srpgClassSprtie
    {
        get { return m_SrpgClassSprite; }
    }

    public ClassInfo classInfo
    {
        get { return m_ClassInfo; }
        set { m_ClassInfo = value; }
    }

    public int level
    {
        get { return m_Level; }
        set { m_Level = value; }
    }

    public SrpgWeapon srpgWeapon
    {
        get { return m_Weapon; }
        set { m_Weapon = value; }
    }

    public SrpgArmor srpgArmor
    {
        get { return m_Armor; }
        set { m_Armor = value; }
    }

    public List<SrpgUseableItem> items
    {
        get { return m_Items; }
        set { m_Items = value; }
    }

    public string weaponName
    {
        get { return m_weaponName; }
        set { m_weaponName = value; }
    }

    public string armorName
    {
        get { return m_armorName; }
        set { m_armorName = value; }
    }

    #endregion

    public SrpgClass(ClassInfo classInfo)
    {
        m_ClassInfo = classInfo;

    }

    public void InitSrpgClass()
    {
        m_Weapon = ItemDatabase.weapon_Dictionary[m_weaponName];
        m_Armor = ItemDatabase.armor_Dictionary[m_armorName];
        if (m_Items == null)
            m_Items = new List<SrpgUseableItem>();
        for(int i = 0; i < m_Items.Count; i++)
        {
            m_Items[i] = ItemDatabase.Items_Dictionary[m_Items[i].m_ItemName];
        }
    }
    //TO DO:技能
}
