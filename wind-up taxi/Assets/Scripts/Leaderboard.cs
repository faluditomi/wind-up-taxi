using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public static Leaderboard Instance { get; private set; }

    private Dictionary<string, int> leaderboard;

    private string leaderboardCode = "leaderboard";
    private string currentNameCode = "currentName";
    private string currentName = "cabbie";

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

            if(PlayerPrefs.HasKey(currentNameCode))
            {
                currentName = PlayerPrefs.GetString(currentNameCode);
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
        Dictionary<string, int> topTen = GetTopTenScores();
        string json = JsonUtility.ToJson(topTen);
        PlayerPrefs.SetString(leaderboardCode, json);
        PlayerPrefs.Save();
    }

    public void AddScore(int score)
    {
        leaderboard.Add(currentName, score);
        SaveLeaderboard();
    }

    public bool HasEntries()
    {
        return leaderboard.Count > 0;
    }

    public Dictionary<string, int> GetTopTenScores()
    {
        if(leaderboard.Count != 0)
        {
            return leaderboard.OrderByDescending(pair => pair.Value).Take(leaderboard.Count).ToDictionary(pair => pair.Key, pair => pair.Value);
        }
        else
        {
            return null;
        }
    }
}
