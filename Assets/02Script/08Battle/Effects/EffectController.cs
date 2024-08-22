using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EffectController : MonoBehaviour
{
    private float lifeTime;
    public float LifeTime
    {
        get => lifeTime;
        set
        {
            if (value < 0f)
                return;

            lifeTime = value;
            timer = 0f;
            isAutoDestroy = true;
        }
    }
    private bool isAutoDestroy = false;
    private float timer = 0f;

    public IEffectGettable target;

    [Header("사운드")]
    public AudioClip enableAudioClip;
    public AudioClip disableAudioClip;
    private AudioSource audioSource;

    private void Awake()
    {
        var soundManager = GameObject.FindWithTag(Defines.Tags.SOUND_MANAGER_TAG)?.GetComponent<SoundManager>();
        audioSource = soundManager?.sfxAudioSource;
    }

    private void OnEnable()
    {
        if (audioSource && enableAudioClip)
            audioSource.PlayOneShot(enableAudioClip);
    }

    private void OnDisable()
    {
        if (audioSource && disableAudioClip)
            audioSource.PlayOneShot(disableAudioClip);
    }

    private void Update()
    {
        AutoDestroy();
        
        var pos = transform.position;
        pos.z = pos.y;
        transform.position = pos;
    }

    private void AutoDestroy()
    {
        if (!isAutoDestroy)
            return;

        timer += Time.deltaTime;
        if (timer > lifeTime)
        {
            ReleaseEffect();
        }
    }

    public void ReleaseEffect()
    {
        if (target != null)
            target.RemoveEffect(this);

        Destroy(gameObject);
    }
}