using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAudio : MonoBehaviour
{
    public static CanvasAudio Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;

    private Dictionary<string, AudioClip> audioClipsDictionary;

    private void Awake()
    {
        Instance = this;

        audioClipsDictionary = new Dictionary<string, AudioClip>();

        foreach (AudioClip audioClip in audioClips)
        {
            audioClipsDictionary.Add(audioClip.name, audioClip);
        }
    }

    public void Play(string audioClipName)
    {
        audioSource.PlayOneShot(audioClipsDictionary[audioClipName]);
    }
}
