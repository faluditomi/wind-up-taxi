using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using FMODUnity;
using FMOD.Studio;
using FMOD;
using System.Diagnostics;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;
    private EventInstance ambienceEventInstance;
    public EventInstance musicEventInstance;

    private bool isInitialised = false;

    private void Awake()
    {
        if(instance != null)
        {
            UnityEngine.Debug.LogError("Found more than one AudioManager in the scene.");
        }
        instance = this;

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();
    }

    private void Start()
    {
        StartAudio();
    }

    void Update()
    {
        if(eventInstances.Count <= 0)
        {
            StartAudio();
        }

        if(!isInitialised && (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
        {
            RESULT result = RuntimeManager.CoreSystem.mixerResume();

            if(result.Equals(RESULT.OK))
            {
                isInitialised = true;
            }
        }
    }

    private void StartAudio()
    {
        // Initializes and starts correct music and ambience
        int currentSceneId = SceneManager.GetActiveScene().buildIndex;
        switch(currentSceneId)
        {
            case 0:
                InitializeMusic(FMODEvents.instance.menuMusic);
                InitializeAmbience(FMODEvents.instance.cityMenu);
                break;

            case 1:
                InitializeMusic(FMODEvents.instance.music);
                InitializeAmbience(FMODEvents.instance.city);
                break;
        }
    }

    private void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateEventInstance(musicEventReference);
        musicEventInstance.start();
    }

    private void InitializeAmbience(EventReference ambienceEventReferece)
    {
        ambienceEventInstance = CreateEventInstance(ambienceEventReferece);
        ambienceEventInstance.start();
    }

    public void SetAmbienceParameter(string parameterName, float parameterValue)
    {
        ambienceEventInstance.setParameterByName(parameterName, parameterValue);
    }

    public void SetMusicArea(MusicArea area)
    {
        musicEventInstance.setParameterByName("area", (float) area);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }

    private void CleanUp()
    {
        // stop and release any created instances
        foreach(EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }

        // stop all of the event emitters, because if we don't, they may hang around in other scenes
        foreach(StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }

}
