using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] Transform startingPos;
    private Transform carTransform;

    [SerializeField] GameObject pausePanel;

    [SerializeField] float remainingTime;
    private float startingTime;

    [SerializeField] DeathMenu deathMenuScript;
    private ChangeDeathScene deathScript;
    private Arrow arrowScript;
    private Car carScript;

    private int score = 0;
    private int minutes;
    private int seconds;

    private bool isPaused;

    private void Awake()
    {
        deathScript = FindAnyObjectByType<ChangeDeathScene>();

        carTransform = GameObject.FindGameObjectWithTag("Car").GetComponent<Transform>();

        arrowScript = FindAnyObjectByType<Arrow>();

        carScript = FindAnyObjectByType<Car>();
    }

    private void Start()
    {
        startingTime = remainingTime;
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

        isPaused = true;

        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);

        isPaused = false;

        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        carScript.Restart();

        carTransform.position = startingPos.position;

        carTransform.localRotation = Quaternion.Euler(0, 0, 0);

        score = 0;

        ResumeGame();

        remainingTime = startingTime;

        arrowScript.ResetMap();
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
