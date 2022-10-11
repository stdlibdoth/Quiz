using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizEntryScript : EntryToggleScript
{
    [SerializeField] private TextMeshProUGUI m_indexText;
    [SerializeField] private TextMeshProUGUI m_nameText;

    public void SetIndexText(int index)
    {
        m_indexText.text = index.ToString();
    }

    public void SetNameText(string name)
    {
        m_nameText.text = name;
    }
}
