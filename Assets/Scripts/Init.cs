using UnityEngine;
using UnityEngine.SceneManagement;

public class Init : MonoBehaviour
{
    public void PlayGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
