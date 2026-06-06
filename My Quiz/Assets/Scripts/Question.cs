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

    int currentIndex = 0;
    int score = 0;
    int totalQuestions = 0;
    int correctCount = 0;

    void Start()
    {
        csvFile = Resources.Load<TextAsset>("Question");

        if (csvFile == null)
        {
            Debug.LogError("CSVが見つかりません");
            return;
        }
        Shuffle(quizList);

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
            quiz.explanation = data[6];

            quizList.Add(quiz);
        }

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

        QuizData quiz = quizList[currentIndex];
        currentQuiz = quiz;
        questionText.text = quiz.question;//テキストで変更

        for (int i = 0; i < 4; i++)
        {
            choiceTexts[i].text= quiz.choices[i];//テキストなどで変更
        }
        currentIndex++;
    }
    public void Answer(int choiceIndex)
    {
        Debug.Log("Answer呼ばれた");
        bool isCorrect = choiceIndex == currentQuiz.correctAnswer;

        totalQuestions++;

        quizCanvas.SetActive(false);
        resultCanvas.SetActive(true);
        Debug.Log("quizCanvas: " + quizCanvas);
        Debug.Log("resultCanvas: " + resultCanvas);

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
        if (currentIndex >= quizList.Count)
        {
            ShowFinalResult();
            return;
        }
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
}
