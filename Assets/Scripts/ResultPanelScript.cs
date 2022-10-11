using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultPanelScript : MonoBehaviour
{
    [SerializeField] private Button m_backBtn;
    [SerializeField] private TextMeshProUGUI m_scoreText;
    [SerializeField] private QuestionBrowsingScript m_questionBrowser;


    private int m_quizIndex;

    private void Awake()
    {
        m_backBtn.onClick.AddListener(BackBtnClickHandler);
    }

    public void ShowScore(int quiz_index, int score)
    {
        m_quizIndex = quiz_index;
        QuizData qzData = GameManager.GetQuizData(m_quizIndex);
        string scoreText = "Your Score:" + score + "/" + qzData.questionData.Count;
        m_scoreText.text = scoreText;
    }

    private void BackBtnClickHandler()
    {
        gameObject.SetActive(false);
        m_questionBrowser.gameObject.SetActive(true);
        m_questionBrowser.SetQuizIndex(m_quizIndex);
        m_questionBrowser.Refresh();
    }
}
