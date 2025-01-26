using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] GameObject pausePanel;

    [SerializeField] float remainingTime;

    [SerializeField] DeathMenu deathMenuScript;
    private ChangeDeathScene deathScript;

    private int score = 0;
    private int minutes;
    private int seconds;

    private void Awake()
    {
        deathScript = FindAnyObjectByType<ChangeDeathScene>();
    }

    private void Update()
    {
        if(remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if(remainingTime < 0)
        {
            remainingTime = 0;

            //Out of time death scene.
            deathScript.ChangeCamera("OutOfTime");
        }

        minutes = Mathf.FloorToInt(remainingTime / 60);

        seconds = Mathf.FloorToInt(remainingTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        scoreText.text = "Happy Customers: " + score;
    }

    public void AddTime(float addedTime)
    {
        remainingTime += addedTime;
    }

    public void AddScore(int addedScore)
    {
        score += addedScore;
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);

        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);

        Time.timeScale = 1;
    }

    public void RestartGame()
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
}
