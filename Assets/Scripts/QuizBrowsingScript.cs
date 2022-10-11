using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizBrowsingScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button m_createBtn;
    [SerializeField] private Button m_deleteBtn;
    [SerializeField] private Button m_editBtn;
    [SerializeField] private Button m_createSaveBtn;
    [SerializeField] private Button m_createCancelBtn;
    [SerializeField] private GameObject m_createPopup;
    [SerializeField] private TMP_InputField m_quizNameInput;
    [SerializeField] private Transform m_entryHolder;
    [SerializeField] private ToggleGroupScript m_toggleGroupScript;
    [SerializeField] private QuestionBrowsingScript m_questionBrowse;

    [Header("Prefabs")]
    [SerializeField] private QuizEntryScript m_entryUIPrefab;

    private List<EntryToggleScript> m_entries;
    private QuizData[] m_data;
    private int m_selectedIndex;

    #region MonoBehaviouors


    private void Awake()
    {
        m_entries = new List<EntryToggleScript>();
        m_createBtn.onClick.AddListener(CreateBtnClickHandler);
        m_deleteBtn.onClick.AddListener(OnDeleteClickHandler);
        m_createCancelBtn.onClick.AddListener(CreateCancelClickHandler);
        m_createSaveBtn.onClick.AddListener(CreateSaveClickHandler);
        m_editBtn.onClick.AddListener(EditBtnClickHandler);
        m_quizNameInput.onValueChanged.AddListener((input) => m_createSaveBtn.interactable = input != "");
    }

    private void Start()
    {
        m_toggleGroupScript.OnAnyToggle.AddListener(QuizEntryToggleHandler);
        GameManager.OnDataChange.AddListener(OnDataChangeHandler);
    }

    private void OnEnable()
    {
        m_toggleGroupScript.SetAllToggleWithOutNotify(false);
        m_editBtn.interactable = false;
    }
    #endregion

    #region Handlers
    private void EditBtnClickHandler()
    {
        int index = m_toggleGroupScript.ActiveEntry.BoundID;
        gameObject.SetActive(false);
        m_questionBrowse.gameObject.SetActive(true);
        m_questionBrowse.SetQuizIndex(index);
        m_questionBrowse.Refresh();
    }

    private void QuizEntryToggleHandler(EntryToggleScript entry)
    {
        m_editBtn.interactable = m_toggleGroupScript.ActiveEntry != null;
        m_deleteBtn.interactable = m_toggleGroupScript.ActiveEntry != null;
    }

    private void CreateBtnClickHandler()
    {
        m_createPopup.SetActive(true);
        m_createSaveBtn.interactable = false;
    }

    private void CreateCancelClickHandler()
    {
        m_createPopup.SetActive(false);
        m_quizNameInput.text = "";
    }

    private void CreateSaveClickHandler()
    {
        int index = GameManager.CreateQuiz(m_quizNameInput.text);
        GameManager.SaveQuiz(index);
        m_createPopup.SetActive(false);
        m_quizNameInput.text = "";
        RefreshInternal();
    }

    private void OnDeleteClickHandler()
    {
        int index = m_toggleGroupScript.ActiveEntry.BoundID;
        GameManager.DeleteQuiz(index);
        RefreshInternal();
    }

    private void OnDataChangeHandler(QuizData[] data)
    {
        m_data = data;
        RefreshInternal();
    }
    #endregion

    #region private methods

    private void RefreshInternal()
    {
        while (m_entries.Count>0)
        {
            RemoveEntry(m_entries[0]);
        }

        for (int i = 0; i < m_data.Length; i++)
        {
            PopulateEntry(m_data[i]);
        }
        m_editBtn.interactable = false;
        m_deleteBtn.interactable = false;
    }

    private void PopulateEntry(QuizData data)
    {
        QuizEntryScript entry = Instantiate(m_entryUIPrefab);
        entry.BoundID = data.index;
        entry.transform.SetParent(m_entryHolder, false);
        m_entries.Add(entry);
        entry.SetIndexText(m_entries.Count);
        entry.SetNameText(data.name);
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
