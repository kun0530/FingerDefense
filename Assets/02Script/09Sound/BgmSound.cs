using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmSound : MonoBehaviour
{
    private MainBgmSound bgmManager;
    public AudioClip bgm;
    public bool isAutoPlay;

    private void OnEnable()
    {
        if (!bgmManager)
            bgmManager = GameObject.FindWithTag(Defines.Tags.SOUND_MANAGER_TAG)?.GetComponentInChildren<MainBgmSound>();

        if (isAutoPlay)
        {
            bgmManager?.PlayAudioClip(bgm);
        }
    }

    private void OnDisable()
    {
        if (isAutoPlay)
        {
            bgmManager?.PlayMainBgm();
        }
    }

    public void PlayBgm()
    {
        bgmManager?.PlayAudioClip(bgm);
    }

    public void PlayMainBgm()
    {
        bgmManager?.PlayMainBgm();
    }
}