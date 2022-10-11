using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class EntryToggleEvent : UnityEvent<EntryToggleScript> { }

public class ToggleGroupScript : MonoBehaviour
{
    [SerializeField] private List<EntryToggleScript> m_entries;


    private EntryToggleScript m_activeEntry;
    private EntryToggleEvent m_onAnyToggle;
    public EntryToggleEvent OnAnyToggle
    {
        get { return m_onAnyToggle; }
    }

    public EntryToggleScript ActiveEntry
    {
        get { return m_activeEntry; }
    }

    public int ActiveEntryIndex
    {
        get
        {
            for (int i = 0; i < m_entries.Count; i++)
            {
                if (m_activeEntry == m_entries[i])
                    return i;
            }
            return -1;
        }
    }

    private void Awake()
    {
        m_onAnyToggle = new EntryToggleEvent();
    }

    private void Start()
    {
        foreach (var entry in m_entries)
        {
            entry.OnSwitchOn.AddListener(OnAnyToggleHandler);
            entry.OnSwitchOff.AddListener(OnAnyToggleHandler);
        }        
    }

    private void OnAnyToggleHandler(EntryToggleScript _entry)
    {
        if (_entry.toggle.isOn)
        {
            foreach (var entry in m_entries)
            {
                if (entry != _entry && entry.toggle.isOn)
                    entry.toggle.SetIsOnWithoutNotify(false);
            }
            m_activeEntry = _entry;
        }
        else
        {
            m_activeEntry = null;
        }
        m_onAnyToggle.Invoke(_entry);
    }

    public void AddEntry(EntryToggleScript entry)
    {
        m_entries.Add(entry);
        entry.OnSwitchOn.AddListener(OnAnyToggleHandler);
        entry.OnSwitchOff.AddListener(OnAnyToggleHandler);
    }

    public void RemoveEntry(EntryToggleScript entry)
    {
        entry.OnSwitchOn.RemoveListener(OnAnyToggleHandler);
        entry.OnSwitchOff.RemoveListener(OnAnyToggleHandler);
        m_entries.Remove(entry);
    }

    public void SetAllToggleWithOutNotify(bool on)
    {
        foreach (var entry in m_entries)
        {
            entry.toggle.SetIsOnWithoutNotify(on);
        }
    }
}
