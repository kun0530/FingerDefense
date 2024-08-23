using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxSound : MonoBehaviour
{
    public enum SfxSoundType
    {
        ACTIVATE_BUTTON_CLICK,
        ENTER_BUTTON_CLICK,
        NON_ACTIVATE_BUTTON_CLICK
    }

    private AudioSource sfxAduioSource;
    public AudioClip activateButtonClickAudioClip;
    public AudioClip enterButtonClickAudioClip;
    public AudioClip nonActivateButtonClickAudioClip;

    private Dictionary<SfxSoundType, AudioClip> sounds = new();

    private void Awake()
    {
        sfxAduioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        sounds.Add(SfxSoundType.ACTIVATE_BUTTON_CLICK, activateButtonClickAudioClip);
        sounds.Add(SfxSoundType.ENTER_BUTTON_CLICK, enterButtonClickAudioClip);
        sounds.Add(SfxSoundType.NON_ACTIVATE_BUTTON_CLICK, nonActivateButtonClickAudioClip);
    }

    [VisibleEnum(typeof(SfxSoundType))]
    public void PlaySfxSound(int soundType)
    {
        if (sfxAduioSource && sounds[(SfxSoundType)soundType])
        {
            sfxAduioSource.PlayOneShot(sounds[(SfxSoundType)soundType]);
        }
    }
}
