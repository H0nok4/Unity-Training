using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class SubTextUIWindow : UIBase
{
    [SerializeField] TMP_Text m_TxtText;//显示的文字
    [SerializeField] Image m_Profile;
    [SerializeField] Image m_BackGroundImage;
    [SerializeField] Image m_IconImage;

    private float m_WordInterval = 0.05f;
    private Coroutine m_WriteCoroutine;
    private string m_Text = string.Empty;//需要显示的文字
    [Serializable]
    public class TextWriteDoneEvent : UnityEvent<SubTextUIWindow> { }

    private TextWriteDoneEvent m_TextWriteDoneEvent = new TextWriteDoneEvent();//写入完成事件，用来通知写入完成

    public TextWriteDoneEvent textWriteDoneEvent
    {
        get 
        { 
            if(m_TextWriteDoneEvent == null)
            {
                 m_TextWriteDoneEvent = new TextWriteDoneEvent();
            }
            return m_TextWriteDoneEvent;
        }
        set { m_TextWriteDoneEvent = value; }
    }
    
    public float wordInterval
    {
        get { return m_WordInterval; }
        set { m_WordInterval = Mathf.Max(0f, value); }
    }

    public bool isWriting
    {
        get { return m_WriteCoroutine != null; }
    }
    //返回应该要写入的完整文本
    public string text
    {
        get { return m_Text; }
    }

    public void OnWriteDone()
    {
        textWriteDoneEvent.Invoke(this);
        DisplayIcon(true) ;
    }

    public TMP_Text txtText
    {
        get { return m_TxtText; }
    }

    public void WriteTextAsync(string text)
    {
        if (isWriting)
        {
            StopCoroutine(m_WriteCoroutine);
            m_WriteCoroutine = null;
        }

        m_Text = text;
        DisplayIcon(false);

        if (txtText == null)
        {
            OnWriteDone();
            return;
        }

        txtText.text = string.Empty;
        if (string.IsNullOrEmpty(m_Text))
        {
            OnWriteDone();
            return;
        }

        m_WriteCoroutine = StartCoroutine(WritingText());
    }
    
    IEnumerator WritingText()
    {
        int index = 0;
        string curText = string.Empty;
        while(txtText.text != m_Text)
        {
            yield return new WaitForSeconds(wordInterval);
            curText += m_Text[index++];
            txtText.text = curText;
        }

        OnWriteDone();
        m_WriteCoroutine = null;
    }

    public void WriteText()
    {
        //如果显示途中点击鼠标，直接显示完整的text
        if (isWriting)
        {
            StopCoroutine(m_WriteCoroutine);
            m_WriteCoroutine = null;

        }

        txtText.text = m_Text;
        OnWriteDone();
    }

    public void WriteText(string text)
    {
        if (isWriting)
        {
            StopCoroutine(m_WriteCoroutine);
            m_WriteCoroutine = null;

        }

        txtText.text = text;
        OnWriteDone();
    }

    private void Awake()
    {
        if(txtText != null)
        {
            m_Text = txtText.text;
        }
    }

    public void OnEnable()
    {
        DisplayIcon(false);
    }

    public void DisplayIcon(bool show)
    {
        if(m_IconImage != null &&   m_IconImage.enabled != show)
        {
            m_IconImage.enabled = show;
        }
    }

    public void Display(bool open)
    {
        if(gameObject.activeSelf != open)
        {
            gameObject.SetActive(open);
        }
    }

}
