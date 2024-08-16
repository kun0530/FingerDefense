using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Slider masterSlider;
    
    public Toggle bgmToggle;
    public Toggle sfxToggle;
    public Toggle MasterToggle;
    
    
    public Toggle vibrationToggle;
    
    public AudioMixer AudioMixer;
    
    
    public void Start()
    {
        bgmSlider.value = PlayerPrefs.GetFloat("BgmVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SfxVolume", 0.5f);
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        bgmToggle.isOn = PlayerPrefs.GetInt("BgmMute", 0) == 1;
        sfxToggle.isOn = PlayerPrefs.GetInt("SfxMute", 0) == 1;
        MasterToggle.isOn = PlayerPrefs.GetInt("MasterMute", 0) == 1;
        vibrationToggle.isOn = PlayerPrefs.GetInt("Vibration", 1) == 1;
        
        bgmSlider.onValueChanged.AddListener(delegate { SetBgmVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { SetSfxVolume(); });
        masterSlider.onValueChanged.AddListener(delegate { SetMasterVolume(); });
        bgmToggle.onValueChanged.AddListener(delegate { SetBgmMute(); });
        sfxToggle.onValueChanged.AddListener(delegate { SetSfxMute(); });
        MasterToggle.onValueChanged.AddListener(delegate { SetMasterMute(); });
        vibrationToggle.onValueChanged.AddListener(delegate { SetVibration(); });
        
    }

    public void Update()
    {
    }

    private void SetBgmVolume()
    {
        float volume = bgmSlider.value;
        AudioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("BGM", volume);
    }

    private void SetSfxVolume()
    {
        float volume = sfxSlider.value;
        AudioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFX", volume);
    }

    private void SetMasterVolume()
    {
        float volume = masterSlider.value;
        AudioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Master", volume);
    }

    private void SetBgmMute()
    {
        int mute = bgmToggle.isOn ? 1 : 0;
        AudioMixer.SetFloat("BGM", mute == 1 ? -80 : Mathf.Log10(bgmSlider.value) * 20);
        PlayerPrefs.SetInt("BGM", mute);
    }

    private void SetSfxMute()
    {
        int mute = sfxToggle.isOn ? 1 : 0;
        AudioMixer.SetFloat("SFX", mute == 1 ? -80 : Mathf.Log10(sfxSlider.value) * 20);
        PlayerPrefs.SetInt("SFX", mute);
    }

    private void SetMasterMute()
    {
        int mute = MasterToggle.isOn ? 1 : 0;
        AudioMixer.SetFloat("Master", mute == 1 ? -80 : Mathf.Log10(masterSlider.value) * 20);
        PlayerPrefs.SetInt("Master", mute);
    }

    private void SetVibration()
    {
        int vibration = vibrationToggle.isOn ? 1 : 0;
        PlayerPrefs.SetInt("Vibration", vibration);
    }
    
    public void OnClickBack()
    {
        gameObject.SetActive(false);
        TimeScaleController.SetTimeScale(1f);
    }

    public void OnClickIn()
    {
        gameObject.SetActive(true);
        TimeScaleController.SetTimeScale(0f);
    }
    
}
