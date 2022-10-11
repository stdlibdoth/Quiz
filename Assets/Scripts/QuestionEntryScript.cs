using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class QuestionEntryScript : EntryToggleScript
{
    [SerializeField] private TextMeshProUGUI m_indexText;
    [SerializeField] private TextMeshProUGUI m_questionText;
    [SerializeField] private TextMeshProUGUI m_answerText;


    public void SetIndexText(int index)
    {
        m_indexText.text = index.ToString();
    }

    public void SetQuestionText(string question)
    {
        m_questionText.text = question;
    }

    public void SetAnswerText(string answer)
    {
        m_answerText.text = answer;
    }
}
