using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeline : MonoBehaviour
{

    public int battleTurns = 0;

    #region 单例
    private static Timeline m_instance;
    public static Timeline instance
    {
        get
        {
            if (m_instance == null)
                m_instance = new Timeline();
            return m_instance;
        }
    }
    #endregion

}
