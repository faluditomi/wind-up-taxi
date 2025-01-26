using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public static Leaderboard Instance { get; private set; }

    private Dictionary<string, int> leaderboard;

    private string leaderboardCode = "leaderboard";

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            if(PlayerPrefs.HasKey(leaderboardCode))
            {
                LoadLeaderboard();
            }
            else
            {
                leaderboard = new Dictionary<string, int>();
            }
        }
        else
        {
            Destroy(gameObject);
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
