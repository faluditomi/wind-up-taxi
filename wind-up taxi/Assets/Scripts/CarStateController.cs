using System.Collections;
using System.Collections.Generic;
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

    private Dictionary<CarState, List<CarState>> allowedStateTransitions;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
            if(newState == CarState.Overcharging)
            {
                print(animator);
                print(1);
                animator.SetBool("Overcharging", true);
            }

            if(currentState == CarState.Overcharging)
            {
                print(2);
                animator.SetBool("Overcharging", false);
            }
            
            currentState = newState;

            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator TimedSetStateBehaviour(float timeToWait, CarStateController.CarState stateToSet)
    {
        yield return new WaitForSeconds(timeToWait);
        SetState(stateToSet);
    }
}
