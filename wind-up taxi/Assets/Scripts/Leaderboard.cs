using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    private Dictionary<string, int> leaderboard;

    private string leaderboardCode = "leaderboard";

    private void Start()
    {
        if(PlayerPrefs.HasKey(leaderboardCode))
        {
            LoadLeaderboard();
        }
        else
        {
            leaderboard = new Dictionary<string, int>();
        }
    }

    private void LoadLeaderboard()
    {
        string json = PlayerPrefs.GetString(leaderboardCode);
        leaderboard = JsonUtility.FromJson<Dictionary<string, int>>(json);
    }

    private void SaveLeaderboard()
    {
        Dictionary<string, int> topTen = GetFirstXScores(10);
        string json = JsonUtility.ToJson(topTen);
        PlayerPrefs.SetString(leaderboardCode, json);
        PlayerPrefs.Save();
    }

    public void AddScore(string name, int score)
    {
        leaderboard.Add(name, score);
        SaveLeaderboard();
    }

    public Dictionary<string, int> GetFirstXScores(int numberOfScoresToGet)
    {
        return leaderboard.OrderByDescending(pair => pair.Value).Take(numberOfScoresToGet).ToDictionary(pair => pair.Key, pair => pair.Value);
    }
}
