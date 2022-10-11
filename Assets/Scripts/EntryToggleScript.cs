using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ToggleEvent : UnityEvent<Toggle> { }

public class EntryToggleScript : MonoBehaviour
{
    [SerializeField] protected Toggle m_toggle;
    [SerializeField] protected TextMeshProUGUI m_text;

    protected EntryToggleEvent m_onSwitchOn;
    protected EntryToggleEvent m_onSwitchOff;
    public EntryToggleEvent OnSwitchOn
    {
        get { return m_onSwitchOn; }
    }

    public EntryToggleEvent OnSwitchOff
    {
        get { return m_onSwitchOff; }
    }
    public Toggle toggle
    {
        get { return m_toggle; }
    }

    public int BoundID { get; set; }

    protected void Awake()
    {
        m_onSwitchOn = new EntryToggleEvent();
        m_onSwitchOff = new EntryToggleEvent();
        m_toggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
                m_onSwitchOn.Invoke(this);
            else
                m_onSwitchOff.Invoke(this);
        });
    }

    public void SetToggleText(string text)
    {
        if(m_text != null)
            m_text.text = text;
    }
}
