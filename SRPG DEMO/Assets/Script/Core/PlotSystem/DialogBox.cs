using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class DialogBox : MonoBehaviour
{
    [SerializeField] TMP_Text dialog_Name;
    [SerializeField] TMP_Text dialog_Text;
    [SerializeField] float m_WordInterval = 0.05f;
    [SerializeField] string m_NeedWriteText = string.Empty;
    [SerializeField] string m_NeedWriteName = string.Empty;
    [SerializeField] GameObject m_NextIcon;
    private Coroutine m_WriteCoroutine = null;

    public float wordInterval
    {
        get { return m_WordInterval; }
        set { m_WordInterval = Mathf.Max(0f, value); }
    }

    public bool isWriting
    {
        get { return m_WriteCoroutine != null; }
    }

    public string needWriteText
    {
        get { return m_NeedWriteText; }
    }
    [Serializable]
    public class TextWriteDoneEvent : UnityEvent<DialogBox> { }
    [Space,SerializeField]
    private TextWriteDoneEvent m_textWriteDoneEvent = new TextWriteDoneEvent();

    public TextWriteDoneEvent textWriteDone
    {
        get
        {   if (m_textWriteDoneEvent == null)
            {
                m_textWriteDoneEvent = new TextWriteDoneEvent();
            }
            return m_textWriteDoneEvent;
        }
        set { m_textWriteDoneEvent = value; }
    }

    protected void OnTextWriteDone()
    {
        GameDirecter.instance.state = ScenarioPlayStatus.wait;
        textWriteDone.Invoke(this);
        DisplayIcon(true);
    }

    public void WriteTextAsync(string dialogText,string dialogName)
    {
        if (isWriting)
        {
            StopCoroutine(m_WriteCoroutine);
            m_WriteCoroutine = null;
        }
        if(dialog_Name != null)
            m_NeedWriteName = dialogName;
        if(dialog_Text != null)
            m_NeedWriteText = dialogText;
        
        DisplayIcon(false);

        if(dialog_Text == null)
        {
            OnTextWriteDone();
            return;
        }
        if(dialog_Text != null)
            dialog_Text.text = string.Empty;
        if(dialog_Name != null)
            dialog_Name.text = string.Empty;
        if (string.IsNullOrEmpty(dialogText))
        {
            OnTextWriteDone();
            return;
        }

        m_WriteCoroutine = StartCoroutine(WritingText());
    }

    private IEnumerator WritingText()
    {
        int index = 0;
        string curText = string.Empty;

        if(dialog_Name != null)
            dialog_Name.text = m_NeedWriteName;
        while(dialog_Text.text != m_NeedWriteText)
        {
            yield return new WaitForSeconds(wordInterval);
            curText += m_NeedWriteText[index++];
            dialog_Text.text = curText;
        }

        OnTextWriteDone();
        m_WriteCoroutine = null;
    }

    public void WriteText()
    {
        if (isWriting)
        {
            StopCoroutine(m_WriteCoroutine);
            m_WriteCoroutine = null;

            if(dialog_Name != null)
                dialog_Name.text = m_NeedWriteName;
            if (dialog_Text != null)
                dialog_Text.text = m_NeedWriteText;
            OnTextWriteDone();
        }
    }

    public void WriteText(string text,string name)
    {
        if (isWriting)
        {
            StopCoroutine(m_WriteCoroutine);
            m_WriteCoroutine = null;
        }
        if (dialog_Name != null)
            dialog_Name.text = name;
        if (dialog_Text != null)
            dialog_Text.text = text;
        OnTextWriteDone();
    }

    public void DisplayIcon(bool m_Active)
    {
        m_NextIcon.SetActive(m_Active);
    }

    public void Display(bool m_Active)
    {
        gameObject.SetActive(m_Active);
    }
}
