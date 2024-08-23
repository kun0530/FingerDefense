using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBgmSound : MonoBehaviour
{
    private AudioSource bgmAudioSource;
    public AudioClip mainBgm;

    private void Awake()
    {
        bgmAudioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        PlayMainBgm();
    }

    public void PlayAudioClip(AudioClip clip)
    {
        if (bgmAudioSource && clip)
        {
            bgmAudioSource.clip = clip;
            bgmAudioSource.Play();
        }
    }

    public void PlayMainBgm()
    {
        if (bgmAudioSource && mainBgm)
        {
            bgmAudioSource.clip = mainBgm;
            bgmAudioSource.Play();
        }
    }
}
