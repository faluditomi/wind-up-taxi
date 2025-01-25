using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Car : MonoBehaviour
{
    private CarStateController carStateController;
    private CarMovementController carMovementController;
    private static PlayerInput playerInput;

    private Coroutine chargeCoroutine;
    private Coroutine timerCoroutine;

    private float currentDistanceToTravel = 0f;
    private float currentTimeToTravel = 0f;
    private float distanceIncrement;
    private float timeIncrement;
    [SerializeField] float maxDistanceToTravel = 5f;
    [SerializeField] float maxTimeToTravel = 2f;
    [SerializeField] float maxTimeToCharge = 8f;
    [SerializeField] float maxTimeToOvercharge = 2f;

    private void Awake()
    {
        carStateController = new CarStateController();
        carMovementController = GetComponent<CarMovementController>();
        playerInput = new PlayerInput();
    }

    private void Start()
    {
        playerInput.Enable();
        playerInput.Gameplay.Charge.started += ChargeStartInput;
        playerInput.Gameplay.Charge.canceled += ChargeEndInput;

        distanceIncrement = maxDistanceToTravel / maxTimeToCharge;
        timeIncrement = maxTimeToTravel / maxTimeToCharge;
    }

    private void FixedUpdate()
    {
        // if(carStateController.GetState() == CarStateController.CarState.Moving)
        // {
            carMovementController.SetSteeringInput(playerInput.Gameplay.Steering.ReadValue<float>());
        // }
        
        carMovementController.SetSpeedingInput(playerInput.Gameplay.Speeding.ReadValue<float>());
    }

    private void ChargeStartInput(InputAction.CallbackContext context) 
    {
        if(chargeCoroutine != null)
        {
            StopCoroutine(chargeCoroutine);
            chargeCoroutine = null;
        }

        chargeCoroutine = StartCoroutine(ChargeBehaviour());
    }

    private void ChargeEndInput(InputAction.CallbackContext context) 
    {
        if(chargeCoroutine != null)
        {
            StopCoroutine(chargeCoroutine);
            chargeCoroutine = null;
        }

        MoveCar();
    }

    private void MoveCar()
    {
        if(chargeCoroutine != null)
        {
            StopCoroutine(chargeCoroutine);
            chargeCoroutine = null;
        }

        if(timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
        
        if(carStateController.GetState() == CarStateController.CarState.Busted)
        {
            Reset();
            return;
        }
            

        if(carStateController.GetState() == CarStateController.CarState.Charging
        || carStateController.GetState() == CarStateController.CarState.Overcharging)
        {
            carStateController.SetState(CarStateController.CarState.Moving);
            //start moving the car
        }
    }

    public void Reset()
    {
        currentDistanceToTravel = 0f;
        currentTimeToTravel = 0f;

        if(carStateController.GetState() == CarStateController.CarState.Busted)
        {
            //GameController.GameOver();
        }
    }

    //started when button is pressed
    private IEnumerator ChargeBehaviour()
    {
        Reset();

        if(!carStateController.SetState(CarStateController.CarState.Charging))
        {
            yield break;
        }

        if(timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        timerCoroutine = StartCoroutine(carStateController.TimedSetStateBehaviour(maxTimeToCharge, CarStateController.CarState.Overcharging));

        while(carStateController.GetState() == CarStateController.CarState.Charging)
        {
            //camera zoom

            //camera shake

            //setting the global variables for car movement
            currentDistanceToTravel += distanceIncrement * Time.deltaTime;
            currentTimeToTravel += timeIncrement * Time.deltaTime;
            
            currentDistanceToTravel = Mathf.Min(currentDistanceToTravel, maxDistanceToTravel);
            currentTimeToTravel = Mathf.Min(currentTimeToTravel, maxTimeToTravel);

            yield return null;
        }

        timerCoroutine = StartCoroutine(carStateController.TimedSetStateBehaviour(maxTimeToOvercharge, CarStateController.CarState.Busted));

        while(carStateController.GetState() == CarStateController.CarState.Overcharging)
        {
            //camera shake

            //car shake

            yield return null;
        }

        if(carStateController.GetState() == CarStateController.CarState.Busted)
        {
            Reset();
        }
    }
}
