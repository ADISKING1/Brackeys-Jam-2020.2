using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameModes { Playing, Paused, GameOver}
    public GameModes gameMode = GameModes.Playing;

    public GameObject[] canvas = new GameObject[3];

    public AudioPlayer audioPlayer;
    public AudioClip PauseClip;
    public AudioClip GameOverClip;

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

    public void SetPlaying()
    {
        canvas[0].SetActive(true);
        canvas[1].SetActive(false);
        canvas[2].SetActive(false);

        gameMode = GameModes.Playing;
        audioPlayer.StopAudio();
    }

    public void SetPaused()
    {
        canvas[0].SetActive(false);
        canvas[1].SetActive(true);
        canvas[2].SetActive(false);

        gameMode = GameModes.Paused;
        audioPlayer.PlayLoopAudio(PauseClip);
    }

    public void SetGameOver()
    {
        canvas[0].SetActive(false);
        canvas[1].SetActive(false);
        canvas[2].SetActive(true);

        gameMode = GameModes.GameOver;
        audioPlayer.PlayLoopAudio(GameOverClip);
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
}