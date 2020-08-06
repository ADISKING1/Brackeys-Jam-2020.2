using System.Collections;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(AudioClip audioClip)
    {
        audioSource.pitch = 1;
        audioSource.clip = audioClip;
        audioSource.Play();
        StartCoroutine(StopLoop());
    }
    public void PlayReverseAudio(AudioClip audioClip)
    {
        audioSource.pitch = -1;
        audioSource.loop = true;
        audioSource.clip = audioClip;
        audioSource.Play();
        StartCoroutine(StopLoop());
    }
    public void PlayLoopAudio(AudioClip audioClip)
    {
        audioSource.loop = true;
        audioSource.pitch = 1;
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void StopAudio()
    {
        audioSource.Stop();
        audioSource.loop = false;
    }


    public IEnumerator StopLoop()
    {
        yield return new WaitForSeconds(0.5f);
        audioSource.loop = false;
    }
}
