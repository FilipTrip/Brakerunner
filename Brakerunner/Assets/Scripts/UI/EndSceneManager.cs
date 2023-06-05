using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EndSceneManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button playButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Keyboard keyboard;
    [SerializeField] private Scoreboard scoreboard;
    
    private void Start()
    {
        keyboard.gameObject.SetActive(false);
        playButton.Select();

        scoreboard.LoadHighscores();
        scoreboard.EnsureNoNames();

        int score = (int)GameManager.Instance.GetScore();
        scoreText.text = score.ToString();

        if (scoreboard.IsTop(score))
        {
            Debug.Log("New highscore!");
            scoreboard.AddHighscore(score);
            keyboard.gameObject.SetActive(true);
            playButton.gameObject.SetActive(false);
            mainMenuButton.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("No new highscore.");
        }

        scoreboard.ReloadScoreboard();
    }

}
