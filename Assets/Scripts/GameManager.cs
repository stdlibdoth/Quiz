using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using System.Security;
public class QuizDataEvent: UnityEvent<QuizData[]> { }

public class GameManager : MonoBehaviour
{
    [SerializeField] private string m_dataDir;
    [SerializeField] private string m_fileExtention;

    private static GameManager m_singleton;

    private Dictionary<int,QuizData> m_quizData;
    private QuizDataEvent m_onDataChange;
    private string m_dataPath;
    private bool m_quizDataChanged;
    private List<string> m_filePaths;

    public static QuizDataEvent OnDataChange
    {
        get { return m_singleton.m_onDataChange; }
    }

    private void Awake()
    {
        if (m_singleton != null)
            Destroy(this);
        else
        {
            m_singleton = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
    }


    private void Start()
    {
        StartCoroutine(LoadQuizData());
    }

    private void Init()
    {
        m_quizData = new Dictionary<int, QuizData>();
        m_onDataChange = new QuizDataEvent();
        m_dataPath = Application.dataPath + "/" + m_dataDir;
    }

    private IEnumerator LoadQuizData()
    {
        yield return null;
        if (!Directory.Exists(m_dataPath))
            Directory.CreateDirectory(m_dataPath);
        print(m_dataPath);
        m_filePaths = new List<string>(Directory.GetFiles(m_dataPath));
        for (int i = 0; i < m_filePaths.Count; i++)
        {
            if (Path.GetExtension(m_filePaths[i]) == m_fileExtention)
                LoadData(m_filePaths[i]);
        }

        QuizData[] data = new QuizData[m_quizData.Count];
        m_quizData.Values.CopyTo(data,0);
        m_onDataChange.Invoke(data);
        yield return null;
    }

    private void LoadData(string path)
    {
        using (StreamReader sr = new StreamReader(path))
        {
            QuizData data = JsonUtility.FromJson<QuizData>(sr.ReadToEnd());
            m_quizData.Add(data.index,data);
        }
    }

    private int GetIndexMax()
    {
        int max = 0;
        foreach (var item in m_quizData)
        {
            if (item.Key > max)
                max = item.Key;
        }
        return max;
    }

    private string GetQuizPath(QuizData data)
    {
        string path = data.index.ToString() + "." + data.name;
        string filePath = m_singleton.m_dataPath + "/" + path + m_singleton.m_fileExtention;
        return filePath;
    }

    public static int CreateQuiz(string name)
    {
        int index = m_singleton.GetIndexMax() + 1;
        QuizData qd = new QuizData();
        qd.index = index;
        qd.name = name;
        qd.questionData = new List<QuestionData>();
        m_singleton.m_quizData.Add(qd.index, qd);
        m_singleton.m_quizDataChanged = true;
        return index;
    }

    public static void DeleteQuiz(int index)
    {
        string filePath = m_singleton.GetQuizPath(m_singleton.m_quizData[index]);
        m_singleton.m_quizData.Remove(index);
        File.Delete(filePath);
        QuizData[] data = new QuizData[m_singleton.m_quizData.Count];
        m_singleton.m_quizData.Values.CopyTo(data, 0);
        m_singleton.m_onDataChange.Invoke(data);
    }

    public static void SaveQuiz(int index)
    {
        string filePath = m_singleton.GetQuizPath(m_singleton.m_quizData[index]);

        if (!File.Exists(filePath))
        {
            FileStream fs = File.Create(filePath);
            fs.Close();
        }
        using (StreamWriter sw = new StreamWriter(filePath))
        {
            string jstring = JsonUtility.ToJson(m_singleton.m_quizData[index], true);
            sw.Write(jstring);
            sw.Close();
        }

        if (m_singleton.m_quizDataChanged)
        {
            m_singleton.m_quizDataChanged = false;
            QuizData[] data = new QuizData[m_singleton.m_quizData.Count];
            m_singleton.m_quizData.Values.CopyTo(data, 0);
            m_singleton.m_onDataChange.Invoke(data);
        }
    }

    public static QuizData GetQuizData(int index)
    {
        if (index < 0 || !m_singleton.m_quizData.ContainsKey(index))
            return null;
        else
        {
            return m_singleton.m_quizData[index];
        }
    }
}
