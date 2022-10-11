using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PreviewPanelScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button m_nextBtn;
    [SerializeField] private Button m_submitBtn;
    [SerializeField] private TextMeshProUGUI m_qNumberText;
    [SerializeField] private TextMeshProUGUI m_questionText;
    [SerializeField] private ToggleGroupScript m_toggleGroupScript;
    [SerializeField] private ResultPanelScript m_resultPanelScript;

    [SerializeField] private List<EntryToggleScript> m_optionEntries;


    private int m_quizIndex;
    private int m_currentQuestionIndex;
    private int m_score;

    private void Awake()
    {
        m_nextBtn.onClick.AddListener(OnNextBtnClick);
        m_submitBtn.onClick.AddListener(OnSubmitBtnClick);
    }

    private void Start()
    {
        m_toggleGroupScript.OnAnyToggle.AddListener(OnOptionToggle);
    }

    private void OnOptionToggle(EntryToggleScript entry)
    {
        if (m_submitBtn.gameObject.activeSelf)
            m_submitBtn.interactable = m_toggleGroupScript.ActiveEntry != null;
        else if (m_nextBtn.gameObject.activeSelf)
            m_nextBtn.interactable = m_toggleGroupScript.ActiveEntry != null;
    }

    private void OnNextBtnClick()
    {
        QuestionData data = GetCurrentQuestionData();
        if (m_toggleGroupScript.ActiveEntryIndex == data.answer)
            m_score++;
        m_currentQuestionIndex++;
        data = GetCurrentQuestionData();
        PopulateQuestionInternal(data);
    }

    private void OnSubmitBtnClick()
    {
        QuestionData data = GetCurrentQuestionData();
        if (m_toggleGroupScript.ActiveEntryIndex == data.answer)
            m_score++;
        print(m_score);

        gameObject.SetActive(false);
        m_resultPanelScript.gameObject.SetActive(true);
        m_resultPanelScript.ShowScore(m_quizIndex, m_score);
    }

    public void StartPreview(int quiz_index)
    {
        m_quizIndex = quiz_index;
        m_currentQuestionIndex = 0;
        PopulateQuestionInternal(GetCurrentQuestionData());
    }

    private void PopulateQuestionInternal(QuestionData data)
    {
        m_toggleGroupScript.SetAllToggleWithOutNotify(false);
        m_submitBtn.interactable = false;
        m_nextBtn.interactable = false;
        QuizData qzData = GameManager.GetQuizData(m_quizIndex);

        m_qNumberText.text = "Question(" + (m_currentQuestionIndex+1) + "/" + qzData.questionData.Count + ")";
        for (int i = 0; i < m_optionEntries.Count; i++)
        {
            if (i < data.options.Length)
            {
                m_optionEntries[i].SetToggleText(data.options[i]);
            }
        }
        m_questionText.text = data.question;
        bool btnCondition = m_currentQuestionIndex == qzData.questionData.Count - 1;
        m_submitBtn.gameObject.SetActive(btnCondition);
        m_nextBtn.gameObject.SetActive(!btnCondition);
    }


    private QuestionData GetCurrentQuestionData()
    {
        QuizData qzData = GameManager.GetQuizData(m_quizIndex);
        return qzData.questionData[m_currentQuestionIndex];
    }
}
