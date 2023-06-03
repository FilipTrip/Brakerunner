using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private GameObject dummyPrefab;
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

    public void PlayDummy(Transform sourceTransform, AudioClip audioClip, float pitch = 1f)
    {
        GameObject dummy = Instantiate(dummyPrefab, sourceTransform.position, Quaternion.identity, transform);
        AudioSource audioSource = dummy.GetComponent<AudioSource>();
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(audioClip);
        Destroy(dummy, audioClip.length + 0.1f);
    }
}
