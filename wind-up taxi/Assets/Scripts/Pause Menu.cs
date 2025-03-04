using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD.Studio;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] Transform startingPos;
    private Transform carTransform;

    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject volumePanel;

    private Bus masterBus;

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

        masterBus = RuntimeManager.GetBus("bus:/");
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

            if(!arrowScript.HasPassanger())
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
        //carScript.Restart();

        //carTransform.position = startingPos.position;

        //carTransform.localRotation = Quaternion.Euler(0, 0, 0);

        //score = 0;

        //ResumeGame();

        //remainingTime = startingTime;

        //arrowScript.ResetMap();

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
        volumePanel.SetActive(true);
    }
}
