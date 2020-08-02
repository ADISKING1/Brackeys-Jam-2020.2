using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneReloader : MonoBehaviour
{
    GameObject Canvas;

    private void Start()
    {
        Canvas = GameObject.Find("Canvas");
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape)) || (Input.GetKeyDown(KeyCode.JoystickButton7)))
        {
            Debug.Log("Clearing DOTween");
            DOTween.Clear(true);

            Debug.Log("reloading scene");
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        if( (Input.GetAxis("Horizontal") == 0) || (Input.GetAxisRaw("Horizontal") == 0) )
        {
            Canvas.SetActive(true);
        }
        else
        {
            Canvas.SetActive(false);
        }
    }
}
