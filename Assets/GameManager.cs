using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public RectTransform[] Panels = new RectTransform[3];

    public static GameManager Instance { get; private set; }

    public enum GameModes { Playing, Paused, GameOver}
    public GameModes gameMode = GameModes.Playing;

    public GameObject[] canvas = new GameObject[3];

    public QuestionManager questionManager;

    public AudioPlayer audioPlayer;
    public AudioClip PauseClip;

    public AudioClip Lose;
    public AudioClip Win;
    public AudioClip EpicWin;

    public Text EndQuote;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Replay()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void MainMenu()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(0);
    }
    public void PlayGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Skip()
    {
        audioPlayer.StopAudio();
    }

    public void SetPlaying()
    {
        Panels[0].DOAnchorPos(Vector2.zero, 0.25f).SetDelay(0.25f);
        Panels[1].DOAnchorPos(new Vector2(0, 1750), 0.25f);
        Panels[2].DOAnchorPos(new Vector2(3000, 0), 0.25f);

        gameMode = GameModes.Playing;
        audioPlayer.StopAudio();
    }

    public void SetPaused()
    {
        questionManager.Confetti.Stop();
        
        Panels[0].DOAnchorPos(new Vector2(0, -1750), 0.25f);
        Panels[1].DOAnchorPos(Vector2.zero, 0.25f).SetDelay(0.25f);
        Panels[2].DOAnchorPos(new Vector2(3000, 0), 0.25f);

        gameMode = GameModes.Paused;
        audioPlayer.PlayLoopAudio(PauseClip);
    }

    public void SetGameOver()
    {
        questionManager.StopAllCoroutines();
        questionManager.Confetti.Stop();
        if (questionManager.Score >= 420)
        {
            questionManager.Confetti.Play();
            if (questionManager.Score >= 690)
            {
                audioPlayer.PlayAudio(EpicWin);
                EndQuote.text = "E P I C\nW I N !";
                EndQuote.color = questionManager.DefaultColour[3];
            }
            else
            {
                audioPlayer.PlayAudio(Win);
                EndQuote.text = "Y O U\nW I N !";
                EndQuote.color = questionManager.DefaultColour[2];
            }
        }
        else
        {
            audioPlayer.PlayAudio(Lose);
            EndQuote.text = "Y O U\nL O S E !";
            EndQuote.color = questionManager.DefaultColour[4];
        }

        Panels[0].DOAnchorPos(new Vector2(0, -1750), 0.25f);
        Panels[1].DOAnchorPos(new Vector2(0, 1750), 0.25f);
        Panels[2].DOAnchorPos(Vector2.zero, 0.25f).SetDelay(0.25f);

        gameMode = GameModes.GameOver;
    }
}