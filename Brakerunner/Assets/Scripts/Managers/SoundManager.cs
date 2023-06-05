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

    public AudioSource PlayDummy(Transform sourceTransform, AudioClip audioClip, float pitch = 1f, float volume = 1f)
    {
        GameObject dummy = Instantiate(dummyPrefab, sourceTransform.position, Quaternion.identity, transform);
        AudioSource audioSource = dummy.GetComponent<AudioSource>();
        audioSource.pitch = pitch;
        audioSource.volume = volume;
        audioSource.PlayOneShot(audioClip);
        Destroy(dummy, audioClip.length + 0.1f);
        return audioSource;
    }

    public AudioSource PlayDummy(Transform sourceTransform, SoundData soundData)
    {
        GameObject dummy = Instantiate(dummyPrefab, sourceTransform.position, Quaternion.identity, transform);
        AudioSource audioSource = dummy.GetComponent<AudioSource>();
        audioSource.pitch = soundData.pitch;
        audioSource.volume = soundData.volume;
        audioSource.PlayOneShot(soundData.audioClip);
        Destroy(dummy, soundData.audioClip.length + 0.1f);
        return audioSource;
    }

    public void FadeOut(AudioSource audioSource, float duration)
    {
        StartCoroutine(Coroutine_FadeOut(audioSource, duration));
    }

    private IEnumerator Coroutine_FadeOut(AudioSource audioSource, float duration)
    {
        while (audioSource && audioSource.volume > 0f)
        {
            audioSource.volume -= Time.deltaTime / duration;
            yield return null;
        }
    }

}
