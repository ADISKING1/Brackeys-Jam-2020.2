using System.Collections;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(AudioClip audioClip)
    {
        StopAudio();

        audioSource.pitch = 1;
        audioSource.clip = audioClip;
        audioSource.Play();
        StartCoroutine(StopLoop());
    }
    public void PlayReverseAudio(AudioClip audioClip)
    {
        StopAudio();
        
        audioSource.pitch = -1;
        audioSource.loop = true;
        audioSource.clip = audioClip;
        audioSource.Play();
        StartCoroutine(StopLoop());
    }
    public void PlayLoopAudio(AudioClip audioClip)
    {
        StopAudio();

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
        yield return new WaitForSeconds(0.2f);
        audioSource.loop = false;
    }
}
