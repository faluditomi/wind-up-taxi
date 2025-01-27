using UnityEngine;

public class LeaderboardFinder : MonoBehaviour
{
    public void SayHiToLeaderboard()
    {
        Leaderboard.Instance.SetCurrentNameInputFieldText();
        Leaderboard.Instance.SetLeaderBoardNamesText();
        Leaderboard.Instance.SetLeaderBoardSoresText();
    }
}
