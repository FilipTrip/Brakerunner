using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        SceneTransitioner.Instance.BeforeLoadScene.AddListener(BeforeLoadScene);

        if (Instance == null)
        {
            Debug.Log("Instance was null");
            Instance = this;
            StartCoroutine(Coroutine_FadeInMusic());
        }

        else
        {
            // This is a new instance while an old one was moved to this scene
            Debug.Log("Instance existed");

            // If same clip
            if (audioSource.clip == Instance.audioSource.clip)
            {
                Debug.Log("Same clip");
                // Let the old instance continue playing
                Instance.transform.parent = Camera.main.transform;
                Destroy(gameObject);
            }

            // If new clip
            else
            {
                Debug.Log("New clip");
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

        while (audioSource.volume < 1f)
        {
            yield return null;
            audioSource.volume += Time.unscaledDeltaTime * 0.5f;
        }

        audioSource.volume = 1f;
    }

}
