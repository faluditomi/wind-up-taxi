using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public static Leaderboard Instance { get; private set; }
    
    private LeaderboardHolder leaderboardHolder;

    private Transform leaderboardView;

    private TextMeshProUGUI leaderboardNamesTextUI;
    private TextMeshProUGUI leaderboardScoresTextUI;
    private TMP_InputField currentNameInputFieldUI;

    private string leaderboardCode = "leaderboardHolder";
    private string currentNameCode = "currentName";
    private string currentName;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLeaderboard();

            if(PlayerPrefs.HasKey(currentNameCode))
            {
                currentName = PlayerPrefs.GetString(currentNameCode);
            }
            
            Instance.SetUpUI();
        }
        else
        {
            Instance.SetUpUI();
            Destroy(gameObject);
        }

    }

    public void SetUpUI()
    {
        leaderboardView = GameObject.Find("Canvas").transform.Find("Leaderboard View");
        leaderboardNamesTextUI = leaderboardView.Find("Leaderboard Names").GetComponent<TextMeshProUGUI>();
        leaderboardScoresTextUI = leaderboardView.Find("Leaderboard Scores").GetComponent<TextMeshProUGUI>();
        currentNameInputFieldUI = leaderboardView.Find("Name Input").Find("Input Field").GetComponent<TMP_InputField>();
    }

    public void SetCurrentNameInputFieldText()
    {
        string name;

        if(currentName.Length == 0)
        {
            name = "Mad Cabbie";
        }
        else
        {
            name = currentName;
        }

        currentNameInputFieldUI.SetTextWithoutNotify(currentName);
    }

    public void SetLeaderBoardNamesText()
    {
        print(1);
        string namesText = "";

        if(HasEntries())
        {
            foreach(LeaderboardEntry entry in leaderboardHolder.entries)
            {
                namesText = namesText + entry.name + "\r\n";
            }
        }
        
        leaderboardNamesTextUI.SetText(namesText);
    }

    public void SetLeaderBoardSoresText()
    {
        string scoresText = "";

        if(HasEntries())
        {
            foreach(LeaderboardEntry entry in leaderboardHolder.entries)
            {
                scoresText = scoresText + entry.score + "\r\n";
            }
        }
        
        leaderboardScoresTextUI.SetText(scoresText);
    }

    public void SetCurrentName(string name)
    {
        currentName = name;
        PlayerPrefs.SetString(currentNameCode, currentName);
        PlayerPrefs.Save();
    }

    private void LoadLeaderboard()
    {
        if(PlayerPrefs.HasKey(leaderboardCode))
        {
            string json = PlayerPrefs.GetString(leaderboardCode);
            leaderboardHolder = JsonUtility.FromJson<LeaderboardHolder>(json);
        }
        else
        {
            leaderboardHolder = new LeaderboardHolder();
        }
    }

    private void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(leaderboardHolder);
        PlayerPrefs.SetString(leaderboardCode, json);
        PlayerPrefs.Save();
    }

    public void AddScore(int score)
    {
        foreach(LeaderboardEntry entry in leaderboardHolder.entries)
        {
            if(entry.name == currentName && entry.score >= score)
            {
                return;
            }
        }

        leaderboardHolder.entries.Add(new LeaderboardEntry(currentName, score));
        leaderboardHolder.entries.Sort((a, b) => b.score.CompareTo(a.score));

        if(leaderboardHolder.entries.Count > 10)
        {
            leaderboardHolder.entries.RemoveRange(10, leaderboardHolder.entries.Count - 10);
        }

        SaveLeaderboard();
    }

    public List<LeaderboardEntry> GetEntries()
    {
        return leaderboardHolder.entries;
    }

    public bool HasEntries()
    {
        return leaderboardHolder.entries.Count > 0;
    }

    [System.Serializable]
    public class LeaderboardEntry
    {
        public string name;
        public int score;

        public LeaderboardEntry(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
    }

    [System.Serializable]
    public class LeaderboardHolder
    {
        public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
    }
}
