using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    //每个角色都有一个BuffManager实例
    private SrpgClassUnit m_Unit;
    private List<Buff> m_Buffs;

    public BuffManager(SrpgClassUnit unit)
    {
        m_Unit = unit;
        m_Buffs = new List<Buff>();
    }

    public List<Buff> buffs
    {
        get { return m_Buffs; }
        private set { m_Buffs = value; }
    }

    public void AddBuff(Buff newBuff)
    {
        bool hasSameBuff = false;
        for(int i = 0; i < m_Buffs.Count; i++)
        {
            if(m_Buffs[i].id == newBuff.id)
            {
                //如果有重复的技能，刷新该技能的回合数，尝试叠加层数
                hasSameBuff = true;
                m_Buffs[i].curDurationTimes = m_Buffs[i].maxDurationTimes;
                if(m_Buffs[i].curOverlayTimes < m_Buffs[i].maxOverlayTimes)
                {
                    m_Buffs[i].curOverlayTimes++;
                }
                break;
            }
        }
        if(!hasSameBuff)
        {
            m_Buffs.Add(newBuff);
            newBuff.OnBuffAdd(m_Unit);
        }

    }

    public void ReduceBuffDuretionTurn()
    {
        for(int i = 0; i < m_Buffs.Count; i++)
        {
            if(m_Buffs[i].durationType == BuffDurationType.limit)
            {
                m_Buffs[i].curDurationTimes--;
            }

        }

    }

    public void RemoveBuff()
    {
        for(int i = m_Buffs.Count - 1; i >= 0 ; i--)
        {
            if (m_Buffs[i].curDurationTimes == 0)
            {
                m_Buffs[i].OnBuffRemove(m_Unit);
                m_Buffs.RemoveAt(i);

            }
        }
    }
}
