using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    public GameManager gameManager;

    public ParticleSystem Confetti;

    public Color CorrectColour;
    public Color IncorrectColour;
    public Color[] DefaultColour = new Color[4];
    public Color Invisible;

    [Space]
    public int CurrentLevel = 0;
    public int Score = 0;
    public static int HighScore;
    public int Point = 10;

    public Text GameOverScoreText;
    public Text ScoreText;
    public Text LevelText;
    public Text PointText;

    [Space]
    public bool timeBtwQ = false;

    public AudioPlayer audioPlayer;

    public enum Modes { Playing, NextQuestion, Gameover}
    public Modes currentMode = Modes.NextQuestion;

    public AudioClip[] Questions;
    public List<AudioClip> unansweredQuestions = new List<AudioClip>();

    public AudioClip currentQuestion;
    public string currentAnswer;
    public string playerAnswer;

    [Space]
    public Button QButton;
    public Text Qtext;

    public GameObject[] OptionsGameObject = new GameObject[4];
    public Text[] OptionsText = new Text[4];
    public Button[] Options = new Button[4];

    // Start is called before the first frame update
    void Start()
    {
        Confetti.Pause();
        unansweredQuestions = Questions.ToList();
        audioPlayer = GetComponent<AudioPlayer>();
        SetQuestion();

        currentMode = Modes.Playing;
        gameManager.SetPlaying();
    }



    public void SetQuestion()
    {
        ClearOptions();
        CurrentLevel++;
        if (unansweredQuestions.Count != 0)
        {
            int currentQuestionIndex = Random.Range(0, unansweredQuestions.Count);

            currentQuestion = unansweredQuestions[currentQuestionIndex];
            currentAnswer = currentQuestion.name.ToString();

            unansweredQuestions.RemoveAt(currentQuestionIndex);

            Debug.Log(currentAnswer);

            timeBtwQ = true;
            PlayQuestion();
            SetUI();
        }
        else
        {
            currentMode = Modes.Gameover;
            Debug.Log("Game Over");
            gameManager.SetGameOver();
        }
    }

    public void SetOptions()
    {
        int correctOption = Random.Range(0, 4);
        for( int i = 0; i < 4; i++ )
        {
            if(i == correctOption)
            {
                OptionsText[i].text = currentAnswer;
            }
            else
            {
                OptionsText[i].text = Questions[Random.Range(0, Questions.Length)].name.ToString();
            }
            OptionsGameObject[i].SetActive(true);
            Options[i].interactable = true;
            OptionsGameObject[i].GetComponent<Image>().color = DefaultColour[i];
        }
        currentMode = Modes.Playing;
    }
    public void ClearOptions()
    {
        for (int i = 0; i < 4; i++)
        {
            OptionsGameObject[i].SetActive(false);
        }
    }

    public void PlayQuestion()
    {
        StartCoroutine(PQuestion());
    }

    public IEnumerator PQuestion()
    {
        if (timeBtwQ)
        {
            yield return new WaitForSeconds(0.5f);
            Point = 10;
        }
        else
        {
            if (Point > 1)
            {
                PointText.color = Color.Lerp(PointText.color, IncorrectColour, 0.1f);
                Point--;
            }
        }
        SetUI();

        Color QtextTemp = Qtext.color;
        Qtext.color = QButton.colors.disabledColor;
        QButton.interactable = false;
        audioPlayer.PlayReverseAudio(currentQuestion);
        yield return new WaitWhile(() => audioPlayer.audioSource.isPlaying);
        QButton.interactable = true;
        Qtext.color = QtextTemp;

        if(timeBtwQ)
        {
            SetOptions();
            timeBtwQ = false;
        }
    }

    public void DecisionPending(int Selected)
    {
        for (int i = 0; i < 4; i++)
        {
            if (OptionsText[i].text == currentAnswer)
                 OptionsGameObject[i].GetComponent<Image>().color = CorrectColour;
            else
                OptionsGameObject[i].SetActive(false);
            if ((i == Selected) && (playerAnswer != currentAnswer))
            {
                OptionsGameObject[i].SetActive(true);
                OptionsGameObject[i].GetComponent<Image>().color = IncorrectColour;
            }
            Options[i].interactable = false;
        }
    }

    public void ChooseA()
    {
        if(currentMode == Modes.Playing)
        {
            playerAnswer = OptionsText[0].text;
            DecisionPending(0);
            currentMode = Modes.NextQuestion;
            StartCoroutine(NextQuestion());
        }
    }
    public void ChooseB()
    {
        if (currentMode == Modes.Playing)
        {
            playerAnswer = OptionsText[1].text;
            DecisionPending(1);
            currentMode = Modes.NextQuestion;
            StartCoroutine(NextQuestion());
        }
    }
    public void ChooseC()
    {
        if (currentMode == Modes.Playing)
        {
            playerAnswer = OptionsText[2].text;
            DecisionPending(2);
            currentMode = Modes.NextQuestion;
            StartCoroutine(NextQuestion());
        }
    }
    public void ChooseD()
    {
        if (currentMode == Modes.Playing)
        {
            playerAnswer = OptionsText[3].text;
            DecisionPending(3);
            currentMode = Modes.NextQuestion;
            StartCoroutine(NextQuestion());
        }
    }

    public IEnumerator NextQuestion()
    {
        if (playerAnswer == currentAnswer)
        {
            Debug.Log("Correct");
            Score += Point;
            Confetti.Play();
        }
        else
        {
            Debug.Log("Wrong");
        }
        SetUI();

        Color QtextTemp = Qtext.color;
        Qtext.color = QButton.colors.disabledColor;
        QButton.interactable = false;
        audioPlayer.PlayAudio(currentQuestion);
        yield return new WaitWhile(() => audioPlayer.audioSource.isPlaying);
        Confetti.Stop();
        QButton.interactable = true;
        Qtext.color = QtextTemp;
        //yield return new WaitWhile(() => audioPlayer.audioSource.isPlaying);
        SetQuestion();
    }

    public void SetUI()
    {
        if (Score > HighScore)
            HighScore = Score;

        LevelText.text = CurrentLevel.ToString() + " / " + Questions.Length.ToString();
        ScoreText.text = Score.ToString();
        PointText.text = "+" + Point.ToString();
        GameOverScoreText.text = "Score: " + Score.ToString() + "\nHighScore: " + HighScore.ToString();
    }
}
