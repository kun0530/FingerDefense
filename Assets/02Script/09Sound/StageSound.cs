using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSound : MonoBehaviour
{
    [SerializeField] private List<AudioClip> bgms;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        audioSource.clip = bgms[Variables.LoadTable.chapterId];
        audioSource.Play();
    }

    private void OnDisable()
    {
        audioSource.Stop();
    }
}
