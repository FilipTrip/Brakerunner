using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Highscore
{
    public string name;
    public int score;

    public Highscore(string name, int score)
    {
        this.name = name;
        this.score = score;
    }

}

[Serializable]
public class Highscores
{
    public List<Highscore> list;
}