using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Diagnostics;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private float gravity = -1;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI multiplierText;
    [SerializeField] private Slider multiplierSlider;
    [SerializeField] private Image multiplierSliderFill;
    [SerializeField] private Color[] multiplierColors;
    [SerializeField] private int scoreMultiplier;
    [SerializeField] private bool running;
    [SerializeField] private Transform worldTransform;

    private static Stopwatch stopwatch;
    private static float score;

    public string GetTimeString()
    {
        return stopwatch.Elapsed.ToString("mm\\:ss\\.FFF");
    }

    public TimeSpan GetTime()
    {
        return stopwatch.Elapsed;
    }

    public float GetScore()
    {
        return score;
    }

    public void StartRun()
    {
        running = true;
        stopwatch = Stopwatch.StartNew();
        score = 0;
    }

    public void StopRun()
    {
        running = false;
        stopwatch.Stop();
    }

    private void Awake()
    {
        Instance = this;
        Physics2D.gravity = new Vector2(0f, gravity);
    }

    private void Start()
    {
        if (!running && SceneManager.GetActiveScene().name == "Game")
        {
            StartRun();
        }
    }

    private void Update()
    {
        if (!running)
            return;
        
        CalculateScoreMultiplier();
        score += Player.Instance.Speed * scoreMultiplier * Time.deltaTime * 0.2f;

        scoreText.text = ((int)score).ToString();
        multiplierText.text = "x" + scoreMultiplier;
        multiplierSlider.value = Player.Instance.GetSpeedPercentage();

        if (Input.GetKeyDown(KeyCode.G))
            RestartRun();

        else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Alpha0))
        {
            StopRun();
            SceneTransitioner.Instance.FadeToMenuScene();
        }

        else if (Input.GetKeyDown(KeyCode.F))
        {
            StopRun();
            SceneTransitioner.Instance.FadeToScene("End");
        }
    }

    /*private void FixedUpdate()
    {
        foreach (Transform child in worldTransform)
        {
            child.transform.position -= new Vector3(Player.Instance.Speed * Time.fixedDeltaTime, 0f, 0f);
        }
    }*/

    private void CalculateScoreMultiplier()
    {
        float speedPercentage = Player.Instance.GetSpeedPercentage();

        if (speedPercentage < 0.33333f)
        {
            if (scoreMultiplier != 1)
            {
                multiplierText.faceColor = multiplierSliderFill.color = multiplierColors[0];
                scoreMultiplier = 1;
            } 
        } 
        else if (speedPercentage < 0.66666f)
        {
            if (scoreMultiplier != 2)
            {
                multiplierText.faceColor = multiplierSliderFill.color = multiplierColors[1];
                scoreMultiplier = 2;
            }
                
        }
        else if (speedPercentage < 1.0f)
        {
            if (scoreMultiplier != 4)
            {
                multiplierText.faceColor = multiplierSliderFill.color = multiplierColors[2];
                scoreMultiplier = 4;
            }
        }
        else
        {
            if (scoreMultiplier != 8)
            {
                multiplierText.faceColor = multiplierSliderFill.color = multiplierColors[3];
                scoreMultiplier = 8;
            }
        }  
    }

    public void RestartRun()
    {
        SceneTransitioner.Instance.FadeReloadActiveScene();
        SoundManager.Instance.Play("Error");
    }

}
