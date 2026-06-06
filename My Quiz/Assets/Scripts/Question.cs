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

    public Text resultText;
    public Text explanationText;

    void Start()
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

    void ShowRandomQuestion()
    {
        int index = Random.Range(0, quizList.Count);

        QuizData quiz = quizList[index];

        currentQuiz = quiz;
        questionText.text = quiz.question;//テキストで変更

        for (int i = 0; i < 4; i++)
        {
            choiceTexts[i].text= quiz.choices[i];//テキストなどで変更
        }
    }
    public void Answer(int choiceIndex)
    {
        Debug.Log("Answer呼ばれた");
        bool isCorrect = choiceIndex == currentQuiz.correctAnswer;

        quizCanvas.SetActive(false);
        resultCanvas.SetActive(true);
        Debug.Log("quizCanvas: " + quizCanvas);
        Debug.Log("resultCanvas: " + resultCanvas);

        if (isCorrect)
        {
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

        ShowRandomQuestion();
    }
}
