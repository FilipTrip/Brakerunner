using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;

    private static float volume;
    public static float Volume => volume;

    private void Start()
    {
        SceneTransitioner.Instance.BeforeLoadScene.AddListener(BeforeLoadScene);

        if (Instance == null)
        {
            volume = 1f;
            Instance = this;
            StartCoroutine(Coroutine_FadeInMusic());
        }

        else
        {
            // This is a new instance while an old one was moved to this scene

            // If same clip
            if (audioSource.clip == Instance.audioSource.clip)
            {
                // Let the old instance continue playing
                Instance.transform.parent = Camera.main.transform;
                Destroy(gameObject);
            }

            // If new clip
            else
            {
                // Replace old instance
                Instance.audioSource.Stop();
                Destroy(Instance.gameObject);
                Instance = this;
                StartCoroutine(Coroutine_FadeInMusic());
            }
        }
    }

    private void BeforeLoadScene()
    {
        transform.parent = null;
        DontDestroyOnLoad(gameObject);
        Physics2D.SyncTransforms();
    }

    private IEnumerator Coroutine_FadeInMusic()
    {
        audioSource.volume = 0f;

        while (audioSource.volume < volume)
        {
            yield return null;
            audioSource.volume += Time.unscaledDeltaTime * 0.5f * volume;
        }

        audioSource.volume = volume;
    }

    public void SetVolume(float volume)
    {
        MusicManager.volume = volume;
        audioSource.volume = volume;
    }

}
