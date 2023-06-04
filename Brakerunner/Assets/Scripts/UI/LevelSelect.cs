using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private Transform levelGrid;
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private int nonLevelScenes;

    public static int LevelCount { get; private set; }

    private void Start()
    {
        CalculateLevelCount();

        for (int i = 0; i < LevelCount; ++i)
        {
            Instantiate(levelButtonPrefab, levelGrid);
        }

        if (levelGrid.childCount > 0)
            levelGrid.GetChild(0).GetComponent<Button>().Select();
    }

    private void OnEnable()
    {
        if (levelGrid.childCount > 0)
            levelGrid.GetChild(0).GetComponent<Button>().Select();
    }

    public void CalculateLevelCount()
    {
        LevelCount = SceneManager.sceneCountInBuildSettings - nonLevelScenes;
    }

}
