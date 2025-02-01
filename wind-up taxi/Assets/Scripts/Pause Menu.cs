using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD.Studio;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI scoreText;

    private Animator scoreTextAnimator;

    [SerializeField] GameObject pausePanel;

    private Bus masterBus;

    [SerializeField] float remainingTime;

    [SerializeField] DeathMenu deathMenuScript;
    private ChangeDeathScene deathScript;
    private Arrow arrowScript;

    private int score = 0;
    private int minutes;
    private int seconds;

    private bool isPaused;

    private void Awake()
    {
        deathScript = FindAnyObjectByType<ChangeDeathScene>();

        arrowScript = FindAnyObjectByType<Arrow>();

        masterBus = RuntimeManager.GetBus("bus:/");

        scoreTextAnimator = scoreText.gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if(remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if(remainingTime < 0)
        {
            remainingTime = 0;

            if(!arrowScript.GetIsPassenger())
            {
                deathScript.ChangeCamera(ChangeDeathScene.Reason.OutOfTime);
            }
            else
            {
                deathScript.ChangeCamera(ChangeDeathScene.Reason.Kidnapping);
            }
        }

        minutes = Mathf.FloorToInt(remainingTime / 60);

        seconds = Mathf.FloorToInt(remainingTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void AddTime(float addedTime)
    {
        remainingTime += addedTime;
    }

    public void AddScore(int addedScore)
    {
        score += addedScore;

        scoreText.text = score.ToString();

        scoreTextAnimator.SetTrigger("ScoreAdded");
    }

    private void OnEnable()
    {
        ResumeGame();
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);

        isPaused = true;

        Time.timeScale = 0;

        masterBus.setPaused(true);
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);

        isPaused = false;

        Time.timeScale = 1;

        masterBus.setPaused(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        ResumeGame();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void VolumeSlider()
    {
        pausePanel.SetActive(false);
    }
}
