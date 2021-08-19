using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Enum_GameEvent
{
    PlayerUnitCauseDamage,PlayerUnitReciveDamage,PlayerUnitDead,EnemyUnitDead
}
public class GameEventSystem
{
    private static object Singleton_Lock = new object(); //锁同步
    private static GameEventSystem m_instance;
    public static GameEventSystem instance
    {

        get
        {
            lock (Singleton_Lock)
            {
                if (m_instance == null)
                {
                    m_instance = new GameEventSystem();
                }

            }
            return m_instance;
        }
    }

}
