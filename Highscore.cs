using System;
using System.Collections.Generic;
using System.IO;

public class HighScore
{
    private const string fileName = "highscores.txt";

    public Dictionary<string, int> Scores { get; set; }

    public HighScore()
    {
        Scores = new Dictionary<string, int>();
        LoadScores();
    }

    public void AddScore(string name, int timeInSeconds)
    {
        Scores[name] = timeInSeconds;
        SaveScores();
    }

    private void LoadScores()
    {
        if (File.Exists(fileName))
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] scoreParts = line.Split(':');
                    Scores[scoreParts[0]] = int.Parse(scoreParts[1]);
                }
            }
        }
    }

    private void SaveScores()
    {
        using (StreamWriter sw = new StreamWriter(fileName))
        {
            foreach (var score in Scores)
            {
                sw.WriteLine($"{score.Key}:{score.Value}");
            }
        }
    }
}
