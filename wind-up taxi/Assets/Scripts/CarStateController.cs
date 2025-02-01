using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class CarStateController : MonoBehaviour
{
    public enum CarState
    {
        Idle,
        Charging,
        Overcharging,
        Moving,
        Busted
    }

    private CarState currentState;
    
    private Animator animator;
    private StudioEventEmitter soundEmitterWinding;
    private StudioEventEmitter soundEmitterRPM;

    private Dictionary<CarState, List<CarState>> allowedStateTransitions;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        soundEmitterWinding = transform.Find("CarAudioWinding").GetComponent<StudioEventEmitter>();
        soundEmitterRPM = transform.Find("CarAudioRPM").GetComponent<StudioEventEmitter>();
    }

    private void Start()
    {
        InitializeStateTransitions();

        currentState = CarState.Idle;
    }

    private void InitializeStateTransitions()
    {
        allowedStateTransitions = new Dictionary<CarState, List<CarState>>
        {
            { CarState.Idle, new List<CarState> { CarState.Charging } },
            { CarState.Charging, new List<CarState> { CarState.Overcharging, CarState.Moving } },
            { CarState.Overcharging, new List<CarState> { CarState.Moving, CarState.Busted } },
            { CarState.Moving, new List<CarState> { CarState.Idle, CarState.Busted } },
            { CarState.Busted, new List<CarState> { CarState.Idle } }
        };
    }

    public CarState GetState()
    {
        return currentState;
    }
    
    public bool SetState(CarState newState)
    {
        if(allowedStateTransitions[currentState].Contains(newState))
        {
            SetStateAnimatorCheck(newState);

            SetStateAudioCheck(newState);
            
            currentState = newState;

            if(currentState == CarState.Busted)
            {
                transform.GetChild(3).GetComponent<BoxCollider>().enabled = false;

                transform.GetComponent<Rigidbody>().isKinematic = true;
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetStateAnimatorCheck(CarState newState)
    {
        if(currentState == CarState.Overcharging)
        {
            animator.SetBool("Overcharging", false);
        }

        if(newState == CarState.Overcharging)
        {
            animator.SetBool("Overcharging", true);
        }
    }

    private void SetStateAudioCheck(CarState newState)
    {
        if(currentState == CarState.Overcharging)
        {
            soundEmitterWinding.SetParameter("Winding", 0f);
        }
        else if(currentState == CarState.Charging)
        {
            soundEmitterWinding.SetParameter("Winding", 0f);
        }
        else if(currentState == CarState.Moving)
        {
            soundEmitterRPM.SetParameter("RPM", 0f);
        }

        if(newState == CarState.Overcharging)
        {
            soundEmitterWinding.SetParameter("Winding", 1f);
        }
        else if(newState == CarState.Charging)
        {
            soundEmitterWinding.SetParameter("Winding", 1f);
        }
    }

    public IEnumerator TimedSetStateBehaviour(float timeToWait, CarStateController.CarState stateToSet)
    {
        yield return new WaitForSeconds(timeToWait);

        SetState(stateToSet);
    }
}
