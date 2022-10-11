using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class QuestionBrowsingScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button m_addBtn;
    [SerializeField] private Button m_deleteBtn;
    [SerializeField] private Button m_editBtn;
    [SerializeField] private Button m_previewBtn;
    [SerializeField] private Button m_backBtn;
    [SerializeField] private Transform m_entryHolder;
    [SerializeField] private ToggleGroupScript m_toggleGroupScript;
    [SerializeField] private QuestionEditPanelScript m_questionEdit;
    [SerializeField] private QuizBrowsingScript m_quizBrowser;
    [SerializeField] private PreviewPanelScript m_previewScript;

    [Header("Prefabs")]
    [SerializeField] private QuestionEntryScript m_entryUIPrefab;

    private List<EntryToggleScript> m_entries;
    private int m_quizIndex;

    #region MonoBebehaviour

    private void Awake()
    {
        m_entries = new List<EntryToggleScript>();
        m_addBtn.onClick.AddListener(AddClickHandler);
        m_deleteBtn.onClick.AddListener(DeleteClickHandler);
        m_editBtn.onClick.AddListener(EditClickHandler);
        m_previewBtn.onClick.AddListener(PreviewClickHandler);
        m_backBtn.onClick.AddListener(BackClickHandler);
    }

    private void Start()
    {
        m_toggleGroupScript.OnAnyToggle.AddListener(QuestionEntryToggleHandler);
        
    }
    #endregion

    #region EventHandlers

    private void AddClickHandler()
    {
        m_questionEdit.gameObject.SetActive(true);
        m_questionEdit.AddQuestionData(m_quizIndex);
    }

    private void DeleteClickHandler()
    {
        int index = m_toggleGroupScript.ActiveEntry.BoundID;
        QuizData quizData = GameManager.GetQuizData(m_quizIndex);
        QuestionData qdata = quizData.GetQuestionData(index);
        quizData.questionData.Remove(qdata);
        GameManager.SaveQuiz(m_quizIndex);
        Refresh();
    }

    private void EditClickHandler()
    {
        m_questionEdit.gameObject.SetActive(true);
        int index = m_toggleGroupScript.ActiveEntry.BoundID;
        m_questionEdit.EditQuestionData(m_quizIndex, index);
    }

    private void PreviewClickHandler()
    {
        m_previewScript.gameObject.SetActive(true);
        m_previewScript.StartPreview(m_quizIndex);
        gameObject.SetActive(false);
    }

    private void BackClickHandler()
    {
        m_quizBrowser.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void QuestionEntryToggleHandler(EntryToggleScript entry)
    {
        m_editBtn.interactable = m_toggleGroupScript.ActiveEntry != null;
        m_deleteBtn.interactable = m_toggleGroupScript.ActiveEntry != null;
    }
    #endregion

    #region public methods
    public void Refresh()
    {
        while (m_entries.Count > 0)
        {
            RemoveEntry(m_entries[0]);
        }

        QuizData quizData = GameManager.GetQuizData(m_quizIndex);
        foreach (var questionData in quizData.questionData)
        {
            PopulateEntry(questionData);
        }
        m_toggleGroupScript.SetAllToggleWithOutNotify(false);
        m_editBtn.interactable = false;
        m_deleteBtn.interactable = false;
        m_previewBtn.interactable = quizData.questionData.Count != 0;
    }

    public void SetQuizIndex(int quiz_index)
    {
        m_quizIndex = quiz_index;
    }
    #endregion

    #region private methods
    private void PopulateEntry(QuestionData data)
    {
        QuestionEntryScript entry = Instantiate(m_entryUIPrefab);
        entry.BoundID = data.index;
        entry.transform.SetParent(m_entryHolder, false);
        m_entries.Add(entry);
        entry.SetIndexText(m_entries.Count);
        entry.SetQuestionText(data.question);
        if(data.options.Length>data.answer && data.answer>=0)
            entry.SetAnswerText(data.options[data.answer]);
        m_toggleGroupScript.AddEntry(entry);
    }

    private void RemoveEntry(EntryToggleScript entry)
    {
        m_entries.Remove(entry);
        m_toggleGroupScript.RemoveEntry(entry);
        Destroy(entry.gameObject);
    }

    #endregion
}
