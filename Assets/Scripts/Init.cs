using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Init : MonoBehaviour
{
    public AudioClip StartClip;
    public AudioClip QuitClip;

    public AudioPlayer audioPlayer;

    public void Awake()
    {
        audioPlayer = GetComponent<AudioPlayer>();
    }
    public void Start()
    {
        audioPlayer.PlayAudio(StartClip);
    }

    public void PlayGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        StartCoroutine(Quit());
    }

    public IEnumerator Quit()
    {
        audioPlayer.PlayAudio(QuitClip);
        yield return new WaitWhile(() => audioPlayer.audioSource.isPlaying);
        Application.Quit();
    }
}
