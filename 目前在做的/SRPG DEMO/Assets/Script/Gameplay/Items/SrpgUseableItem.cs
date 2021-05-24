using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum ItemUseTarget
{
    self,
    ally,
    enemy
}
[Serializable]
public class SrpgUseableItem : item
{
    public string m_ItemName;
    public ItemUseTarget m_ItemUseTarget;
    public int[][] m_UseRenge;
    public string m_Des;

    public virtual void Execute(SrpgClassUnit srpgClass)
    {

        
    }
}
