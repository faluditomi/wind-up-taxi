using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI reasonText;
    [SerializeField] TextMeshProUGUI finalScoreText;

    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetReason(string reason)
    {
        reasonText.text = reason;
    }

    public void SetFinalScore(string score)
    {
        finalScoreText.text = "Debit Cards Charged:\r\n" + score;
    }
}
