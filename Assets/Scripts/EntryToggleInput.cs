using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EntryToggleInput : EntryToggleScript
{
    [SerializeField] private TMP_InputField m_inputField;

    public TMP_InputField InputField
    {
        get { return m_inputField; }
    }
}
