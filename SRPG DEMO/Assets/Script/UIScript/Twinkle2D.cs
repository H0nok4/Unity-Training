using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Twinkle2D : MonoBehaviour
{
    //闪烁
    [SerializeField] Image m_image;
    [SerializeField] float m_TwinkleInterval = 0.5f;

    private Color m_OldColor;
    private bool m_Hide = false;
    private float m_Timer = 0f;

    private void OnEnable()
    {
        if(m_image != null)
        {
            m_OldColor = m_image.color;
        }
    }

    private void OnDisable()
    {
        if(m_image != null)
        {
            m_image.color = m_OldColor;
        }
    }

    public void Update()
    {
        if(m_image == null && !m_image.enabled)
        {
            return;
        }

        m_Timer += Time.deltaTime;
        if(m_Timer > m_TwinkleInterval)
        {
            m_Timer -= m_TwinkleInterval;

            m_image.color = m_Hide ? Color.clear : m_OldColor;
            m_Hide = !m_Hide;
        }
    }
}
