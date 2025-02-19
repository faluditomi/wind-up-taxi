using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD.Studio;

public class MainMenu : MonoBehaviour
{
    private Bus masterBus;

    private EventInstance menuMusic;
    private EventInstance menuAmbience;

    private void Start()
    {
        menuMusic = AudioManager.instance.CreateEventInstance(FMODEvents.instance.menuMusic);
        menuAmbience = AudioManager.instance.CreateEventInstance(FMODEvents.instance.cityMenu);
        masterBus = RuntimeManager.GetBus("bus:/");
        masterBus.setPaused(false);

        menuMusic.start();
        menuAmbience.start();
    }

    public void PlayGame()
    {
        menuMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        menuMusic.release();
        menuAmbience.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        menuAmbience.release();

        //todo: Update this when we have a main map.
        SceneManager.LoadSceneAsync(1);
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
