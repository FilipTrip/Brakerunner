using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneTransitioner : MonoBehaviour
{
    public static SceneTransitioner Instance { get; private set; }

    [SerializeField] private Animator animator;
    [SerializeField] private float fadeDuration;
    [SerializeField] private float loadSceneDelay;

    public UnityEvent BeforeLoadScene = new UnityEvent();

    private void Awake()
    {
        Instance = this;
    }

    public void FadeToScene(string sceneName)
    {
        animator.SetTrigger("FadeToBlack");
        DelayedCall.Create(this, () => LoadScene(sceneName), fadeDuration + loadSceneDelay);
    }

    public void FadeToMenuScene()
    {
        animator.SetTrigger("FadeToBlack");
        DelayedCall.Create(this, LoadMenuScene, fadeDuration + loadSceneDelay);
    }

    public void FadeReloadActiveScene()
    {
        animator.SetTrigger("FadeToBlack");
        DelayedCall.Create(this, ReloadActiveScene, fadeDuration + loadSceneDelay);
    }

    public void FadeExitGame()
    {
        animator.SetTrigger("FadeToBlack");
        DelayedCall.Create(this, Exit.ExitApplication, fadeDuration + loadSceneDelay);
    }

    public void ReloadActiveScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMenuScene()
    {
        LoadScene("Menu");
    }

    public void LoadScene(string sceneName)
    {
        BeforeLoadScene.Invoke();
        SceneManager.LoadScene(sceneName);
    }
}
