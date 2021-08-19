using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Twinkle2DText : MonoBehaviour
{
    //闪烁
    [SerializeField] TMP_Text m_text;
    [SerializeField] float m_TwinkleInterval = 10f;

    private Color m_OldColor;
    [SerializeField] bool m_Reverse = true;
    [SerializeField] float m_Timer = 0f;

    private void OnEnable()
    {
        if (m_text != null)
        {
            m_OldColor = m_text.color;
        }
    }

    private void OnDisable()
    {
        if (m_text != null)
        {
            m_text.color = m_OldColor;
        }
    }

    public void Update()
    {
        if (m_text == null && !m_text.enabled)
        {
            return;
        }

        m_Timer += Time.deltaTime;


        if (m_Timer > m_TwinkleInterval)
        {
            m_Reverse = !m_Reverse;
            m_Timer -= m_TwinkleInterval;
        }

        if (m_Reverse)
        {
            m_text.color = new Color(m_text.color.r, m_text.color.g, m_text.color.b, Mathf.Lerp(0f, 1f, m_text.color.a - 0.0035f));

        }
        else
        {
            m_text.color = new Color(m_text.color.r, m_text.color.g, m_text.color.b, Mathf.Lerp(0f, 1f, m_text.color.a + 0.004f));
        }

    }
}
