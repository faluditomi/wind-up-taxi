using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public static Leaderboard Instance { get; private set; }

    private Dictionary<string, int> leaderboard;

    private TextMeshProUGUI leaderboardNamesTextUI;
    private TextMeshProUGUI leaderboardScoresTextUI;
    private TMP_InputField currentNameInputFieldUI;

    private string leaderboardCode = "leaderboard";
    private string currentNameCode = "currentName";
    private string currentName = "mad cabbie";

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

            Transform leaderBoardView = GameObject.Find("Leaderboard View").transform;
            leaderboardNamesTextUI = leaderBoardView.Find("Leaderboard Names").GetComponent<TextMeshProUGUI>();
            leaderboardScoresTextUI = leaderBoardView.Find("Leaderboard Scores").GetComponent<TextMeshProUGUI>();
            currentNameInputFieldUI = leaderBoardView.Find("Name Input").Find("Input Field").GetComponent<TMP_InputField>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCurrentNameInputFieldText()
    {
        currentNameInputFieldUI.SetTextWithoutNotify(currentName);
    }

    public void SetLeaderBoardNamesText()
    {
        string namesText = "";

        foreach(KeyValuePair<string, int> entry in Leaderboard.Instance.GetTopTenScores())
        {
            string name = "";

            if(entry.Key.Length > 11)
            {
                name = entry.Key.Substring(0, 11) + "...";
            }
            else
            {
                name = entry.Key;
            }

            namesText.Concat(name + "\r\n");
        }

        leaderboardNamesTextUI.SetText(namesText);
    }

    public void SetLeaderBoardSoresText()
    {
        string scoresText = "";

        foreach(KeyValuePair<string, int> entry in Leaderboard.Instance.GetTopTenScores())
        {
            scoresText.Concat(entry.Value + "\r\n");
        }

        leaderboardScoresTextUI.SetText(scoresText);
    }

    public void SetCurrentName(string name)
    {
        currentName = name;
        PlayerPrefs.SetString(currentNameCode, currentName);
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
