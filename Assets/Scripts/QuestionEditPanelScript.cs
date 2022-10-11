using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionEditPanelScript : MonoBehaviour
{
    [SerializeField] private ToggleGroupScript m_toggleGroupScript;
    [SerializeField] private Button m_submitBtn;
    [SerializeField] private QuestionBrowsingScript m_questionBrowse;
    [SerializeField] private TMP_InputField m_questionInput;
    [SerializeField] private TextMeshProUGUI m_questionHeader;

    [SerializeField] private List<EntryToggleInput> m_optionEntries;

    private int m_quizIndex;
    private int m_questionIndex;
    private bool m_b0;
    private bool m_b1;
    private bool m_b2;

    #region MonoBehaviour

    private void Awake()
    {
        m_submitBtn.interactable = false;
        m_submitBtn.onClick.AddListener(OnSubmitClickHandler);
        m_questionInput.onValueChanged.AddListener((text) => { m_b1 = text != ""; m_submitBtn.interactable = m_b0 && m_b1 && m_b2; });
        foreach (var input in m_optionEntries)
        {
            input.InputField.onValueChanged.AddListener(OnInputChanged);
        }
    }

    private void Start()
    {
        m_toggleGroupScript.OnAnyToggle.AddListener(OnOptionsToggle);      
    }

    private void OnEnable()
    {
        m_toggleGroupScript.SetAllToggleWithOutNotify(false);
    }
    #endregion

    #region Handlers
    private void OnOptionsToggle(EntryToggleScript _entry)
    {
        m_b0 = m_toggleGroupScript.ActiveEntry != null;
        m_submitBtn.interactable = m_b0 && m_b1 && m_b2;
    }


    private void OnSubmitClickHandler()
    {
        int index = m_toggleGroupScript.ActiveEntryIndex;
        QuizData qzData = GameManager.GetQuizData(m_quizIndex);
        QuestionData data = qzData.GetQuestionData(m_questionIndex);

        data.question = m_questionInput.text;
        data.answer = index;
        data.index = m_questionIndex;
        data.quizIndex = m_quizIndex;
        for (int i = 0; i < data.options.Length; i++)
        {
            data.options[i] = m_optionEntries[i].InputField.text;
        }
        GameManager.SaveQuiz(m_quizIndex);
        m_questionBrowse.Refresh();
        gameObject.SetActive(false);
    }

    private void OnInputChanged(string input)
    {
        m_b2 = true;
        foreach (var entry in m_optionEntries)
        {
            if (entry.InputField.text == "")
                m_b2 = false;
        }
        m_submitBtn.interactable = m_b0 && m_b1 && m_b2;
    }
    #endregion

    #region public methods

    public int AddQuestionData(int quiz_index)
    {
        m_quizIndex = quiz_index;
        QuizData qzData = GameManager.GetQuizData(quiz_index);
        QuestionData data = qzData.CreateQuestion(m_optionEntries.Count);
        m_questionIndex = data.index;
        m_toggleGroupScript.SetAllToggleWithOutNotify(false);
        m_questionHeader.text = "Add Question";
        PopulateQuestion(data);
        return m_questionIndex;
    }

    public void EditQuestionData(int quiz_index, int question_index)
    {
        m_quizIndex = quiz_index;
        m_questionIndex = question_index;
        QuizData qzData = GameManager.GetQuizData(quiz_index);
        QuestionData data = qzData.GetQuestionData(question_index);
        m_questionHeader.text = "Edit Question";
        PopulateQuestion(data);
    }
    #endregion

    #region private methods

    private void PopulateQuestion(QuestionData data)
    {
        for (int i = 0; i < m_optionEntries.Count; i++)
        {
            if (i < data.options.Length)
            {
                m_optionEntries[i].InputField.text = data.options[i];
            }
        }
        m_questionInput.text = data.question;
        m_submitBtn.interactable = false;
    }

    #endregion
}
