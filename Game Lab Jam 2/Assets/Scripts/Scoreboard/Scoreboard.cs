using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Linq;
using System.IO;
using UnityEngine.Playables;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] private Keyboard keyboard;
    [SerializeField] private int numberOfHighscores;
    [SerializeField] private Transform highscoresParent;
    [SerializeField] private GameObject highscorePrefab;
    [SerializeField] private Color newHighscoreColor;
    [SerializeField] private Highscores highscores;

    private const string file = "Highscores.json";

    private void Awake()
    {
        if (File.Exists(GetFilePath(file)) == false)
        {
            // Will create file
            SaveHighscores();
        }
    }

    public void LoadHighscores()
    {
        string json = ReadFromFile(file);
        Debug.Log("Reading: " + GetFilePath(file) + "\n" + json);
        highscores = JsonUtility.FromJson<Highscores>(json);
    }

    public void SaveHighscores()
    {
        string json = JsonUtility.ToJson(highscores);
        Debug.Log("Writing: " + GetFilePath(file) + "\n" + json);
        WriteToFile(file, json);
    }

    private void WriteToFile(string fileName, string json)
    {
        string path = GetFilePath(fileName);
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }
    }

    private string ReadFromFile(string fileName)
    {
        string path = GetFilePath(fileName);
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        else
        {
            Debug.LogWarning("File not found");
        }

        return null;
    }

    private string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }

    public void EnsureNoNames()
    {
        for (int i = 0; i < highscores.list.Count; ++i)
        {
            if (highscores.list[i].name == "")
                highscores.list[i].name = "NO NAME";
        }
    }

    public bool IsTop(int score)
    {
        if (highscores.list.Count < numberOfHighscores)
            return true;

        foreach (Highscore highscore in highscores.list)
        {
            if (score > highscore.score)
                return true;
        }
        return false;
    }

    public void AddHighscore(int score)
    {
        highscores.list.Add(new Highscore("", score));
        highscores.list = highscores.list.OrderBy(item => -item.score).ToList();

        while (highscores.list.Count > numberOfHighscores)
        {
            highscores.list.RemoveAt(numberOfHighscores);
        }
    }

    public void SetNewHighscoreName(string name)
    {
        if (name == "")
            name = "NO NAME";

        for (int i = 0; i < highscores.list.Count; ++i)
        {
            if (string.IsNullOrEmpty(highscores.list[i].name))
            {
                highscores.list[i].name = name;
                return;
            }
        }
    }

    public void ReloadScoreboard()
    {
        foreach (Transform transform in highscoresParent)
        {
            Destroy(transform.gameObject);
        }

        TextMeshProUGUI numberText, nameText, scoreText;
        

        for (int i = 0; i < highscores.list.Count; ++i)
        {
            GameObject highscore = Instantiate(highscorePrefab, highscoresParent);

            numberText = highscore.transform.Find("Number").GetComponent<TextMeshProUGUI>();
            nameText = highscore.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            scoreText = highscore.transform.Find("Score").GetComponent<TextMeshProUGUI>();
            Button button = highscore.GetComponentInChildren<Button>();

            numberText.text = (i + 1) + ".";
            nameText.text = highscores.list[i].name;
            scoreText.text = highscores.list[i].score.ToString();

            button.onClick.AddListener(() => OnRemoveButtonClicked(button));

            if (highscores.list[i].name == "")
            {
                numberText.color = newHighscoreColor;
                nameText.color = newHighscoreColor;
                scoreText.color = newHighscoreColor;
                keyboard.SetInputField(nameText);
            }
        }
    }

    public void OnRemoveButtonClicked(Button caller)
    {
        int index = caller.transform.parent.GetSiblingIndex();
        Debug.Log("Removing highscore " + (index + 1));
        highscores.list.RemoveAt(index);
        SaveHighscores();
        ReloadScoreboard();
    }

}
