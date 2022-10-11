using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestionData
{
    public int index;
    public string question;
    public string[] options;
    public int answer;
    public int quizIndex;


}


[System.Serializable]
public class QuizData
{
    public int index;
    public string name;
    public List<QuestionData> questionData;

    public int qIndexCount;

    public QuizData()
    {
        questionData = new List<QuestionData>();
        qIndexCount = 0;
    }
    public QuestionData GetQuestionData(int qIndex)
    {
        for (int i = 0; i < questionData.Count; i++)
        {
            if (questionData[i].index == qIndex)
                return questionData[i];
        }
        return null;
    }

    public QuestionData CreateQuestion(int options)
    {
        QuestionData data = new QuestionData();
        data.options = new string[options];
        questionData.Add(data);
        data.index = qIndexCount;
        qIndexCount++;
        return data;
    }
}
