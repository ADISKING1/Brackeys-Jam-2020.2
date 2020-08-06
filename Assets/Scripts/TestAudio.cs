using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAudio : MonoBehaviour
{
    private AudioPlayer audioPlayer;

    public AudioClip audioClip;

    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<AudioPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            audioPlayer.PlayAudio(audioClip);
        if (Input.GetKeyDown(KeyCode.R))
            audioPlayer.PlayReverseAudio(audioClip);
    }
}
