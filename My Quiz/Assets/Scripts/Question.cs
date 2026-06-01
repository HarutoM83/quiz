using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Question : MonoBehaviour
{
    TextAsset csvFile;

    List<QuizData> quizList = new List<QuizData>();

    void Start()
    {
        csvFile = Resources.Load<TextAsset>("QuestionCSV");

        if (csvFile == null)
        {
            Debug.LogError("CSVが見つかりません");
            return;
        }

        StringReader reader = new StringReader(csvFile.text);

        bool isFirstLine = true;

        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();

            // ヘッダー行をスキップ
            if (isFirstLine)
            {
                isFirstLine = false;
                continue;
            }

            string[] data = line.Split(',');

            QuizData quiz = new QuizData();

            quiz.question = data[0];

            quiz.choices = new string[4];
            quiz.choices[0] = data[1];
            quiz.choices[1] = data[2];
            quiz.choices[2] = data[3];
            quiz.choices[3] = data[4];

            quiz.correctAnswer = int.Parse(data[5]);

            quizList.Add(quiz);
        }

        ShowRandomQuestion();
    }

    void ShowRandomQuestion()
    {
        int index = Random.Range(0, quizList.Count);

        QuizData quiz = quizList[index];

        Debug.Log(quiz.question);

        for (int i = 0; i < 4; i++)
        {
            Debug.Log($"{i + 1}: {quiz.choices[i]}");
        }

        Debug.Log($"正解は {quiz.correctAnswer}");
    }
}
