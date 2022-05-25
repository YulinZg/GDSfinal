using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    public static BGMController instance = null;

    private AudioSource audioSource;
    [SerializeField] AudioClip[] clips;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMenu()
    {
        if (audioSource.clip != clips[0])
            audioSource.clip = clips[0];
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    public void PlayBGM()
    {
        if (audioSource.clip != clips[1])
            audioSource.clip = clips[1];
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    public void PlayEnding()
    {
        if (audioSource.clip != clips[2])
            audioSource.clip = clips[2];
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }
}
