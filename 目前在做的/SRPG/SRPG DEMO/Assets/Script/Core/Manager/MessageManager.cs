using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager
{
    public static MessageManager m_instance;
    public MessageManager instance
    {
        get 
        { if(m_instance == null)
          {
            m_instance = this;
          }
          return m_instance;                   
        }
    }
}
