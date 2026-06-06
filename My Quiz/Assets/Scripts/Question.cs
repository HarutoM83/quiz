using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Question : MonoBehaviour
{
    TextAsset csvFile;

    List<QuizData> quizList = new List<QuizData>();
    public GameObject quizCanvas;
    public Button[] choiceButtons;
    public Text questionText;
    public Text[] choiceTexts;
    QuizData currentQuiz;
    public GameObject resultCanvas;
    public GameObject finalCanvas;
    public Text finalText;

    public Text resultText;
    public Text explanationText;

    int totalQuestions = 0;
    int correctCount = 0;
    int questionNumber = 0;

    void Start()
    {
        quizCanvas.SetActive(true);
        resultCanvas.SetActive(false);
        finalCanvas.SetActive(false);

        LoadCSV();
        Shuffle(quizList);

        ShowRandomQuestion();
    }
    void Shuffle(List<QuizData> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            QuizData temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    void ShowRandomQuestion()
    {
        questionNumber++;
        if (quizList.Count == 0)
        {
            ShowFinalResult();
            return;
        }

        int index = Random.Range(0, quizList.Count);

        QuizData quiz = quizList[index];
        quizList.RemoveAt(index); // ←これ重要

        currentQuiz = quiz;

        questionText.text = quiz.question;

        for (int i = 0; i < 4; i++)
        {
            choiceTexts[i].text = quiz.choices[i];
        }
    }
    public void Answer(int choiceIndex)
    {
        /*
        bool isCorrect = false;

        if (questionNumber <= 3)
        {
            isCorrect = (choiceIndex == 1);
        }
        else if (questionNumber <= 6)
        {
            isCorrect = (choiceIndex == 3);
        }
        else
        {
            isCorrect = (choiceIndex == currentQuiz.correctAnswer);
        }
        */
        bool isCorrect = choiceIndex == currentQuiz.correctAnswer;

        totalQuestions++;

        quizCanvas.SetActive(false);
        resultCanvas.SetActive(true);

        if (isCorrect)
        {
            correctCount++;
            resultText.text = "正解！";
        }
        else
        {
            resultText.text = "不正解！";
        }

        explanationText.text =
        currentQuiz.explanation.Replace("|", "\n");

    }

    public void NextQuestion()
    {
        quizCanvas.SetActive(true);
        resultCanvas.SetActive(false);
        finalCanvas.SetActive(false);

        ShowRandomQuestion();
    }
    void ShowFinalResult()
    {
        quizCanvas.SetActive(false);
        resultCanvas.SetActive(false);
        finalCanvas.SetActive(true);

        float accuracy = (float)correctCount / totalQuestions * 100f;

        finalText.text =
            $"終了！\n\n正解数：{correctCount}/{totalQuestions}\n正答率：{accuracy:F1}%";
    }
    public void Retry()
    {
        correctCount = 0;
        totalQuestions = 0;
        quizList.Clear();
        LoadCSV();
        Shuffle(quizList);

        finalCanvas.SetActive(false);
        resultCanvas.SetActive(false);
        quizCanvas.SetActive(true);
        ShowRandomQuestion();
    }
    void LoadCSV()
    {
        csvFile = Resources.Load<TextAsset>("Question");

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
            quiz.explanation = data[6];

            quizList.Add(quiz);
        }
    }

}
