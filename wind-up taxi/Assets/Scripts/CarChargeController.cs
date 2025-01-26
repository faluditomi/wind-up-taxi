using System.Collections;
using UnityEngine;

public class Car : MonoBehaviour
{
    private CarStateController carStateController;
    private CarMovementController carMovementController;

    private Rigidbody myRigidBody;

    private Coroutine chargeCoroutine;
    private Coroutine timerCoroutine;
    private Coroutine moveCoroutine;

    private float currentMotorForceMultiplier;
    private float currentTimeToTravel;
    [SerializeField] float maxTimeToTravel = 2f;
    [SerializeField] float maxTimeToCharge = 8f;
    [SerializeField] float maxTimeToOvercharge = 2f;
    [SerializeField] float minTimeToTravel = 0.75f;
    [SerializeField] float minMotorForceMultiplier = 20f;

    [SerializeField] bool isInCarMode = false;

    private void Awake()
    {
        carStateController = GetComponent<CarStateController>();
        carMovementController = GetComponent<CarMovementController>();
        myRigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        currentMotorForceMultiplier = minMotorForceMultiplier;
        currentTimeToTravel = minTimeToTravel;
    }

    private void Update()
    {
        GetInput();
    }

    private void GetInput() {
        if(isInCarMode)
        {
            carMovementController.modifyCurrentMotorForce(1f);
            carMovementController.setHorizontalInput(Input.GetAxis("Horizontal"));
            carMovementController.setVerticalInput(Input.GetAxis("Vertical"));
            carMovementController.setBreakingInput(Input.GetKey(KeyCode.Space));
        }
        else
        {
            if(carStateController.GetState() == CarStateController.CarState.Moving)
            {
                carMovementController.setHorizontalInput(Input.GetAxis("Horizontal"));
            }
        
            if(Input.GetKeyDown(KeyCode.Space)
            && carStateController.GetState() == CarStateController.CarState.Idle)
            {
                chargeCoroutine = StartCoroutine(ChargeBehaviour());
            }

            if(Input.GetKeyUp(KeyCode.Space)
            && (carStateController.GetState() == CarStateController.CarState.Charging
            || carStateController.GetState() == CarStateController.CarState.Overcharging))
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

                moveCoroutine = StartCoroutine(MoveBehaviour());
            }
        }
    }

    public void Reset()
    {
        carMovementController.modifyCurrentMotorForce(0f);
        currentMotorForceMultiplier = minMotorForceMultiplier / 100f;
        currentTimeToTravel = minTimeToTravel;

        if(carStateController.GetState() == CarStateController.CarState.Busted)
        {
            //GameController.GameOver();
        }
    }

    private IEnumerator MoveBehaviour()
    {
        carStateController.SetState(CarStateController.CarState.Moving);
        carMovementController.modifyCurrentMotorForce(currentMotorForceMultiplier);
        carMovementController.setVerticalInput(1f);
        
        yield return new WaitForSeconds(currentTimeToTravel);

        carMovementController.modifyCurrentMotorForce(0);
        carMovementController.setVerticalInput(0);
        carMovementController.setBreakingInput(true);

        yield return new WaitUntil(() => myRigidBody.linearVelocity.sqrMagnitude <= 0.1f);

        carMovementController.setBreakingInput(false);
        myRigidBody.linearVelocity = Vector3.zero;
        myRigidBody.angularVelocity = Vector3.zero;
        carStateController.SetState(CarStateController.CarState.Idle);
        moveCoroutine = null;
    }

    private IEnumerator ChargeBehaviour()
    {
        Reset();

        carStateController.SetState(CarStateController.CarState.Charging);

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

            currentMotorForceMultiplier += (1f - (minMotorForceMultiplier / 100f)) / maxTimeToCharge * Time.deltaTime;
            currentTimeToTravel += (maxTimeToTravel - minTimeToTravel) / maxTimeToCharge * Time.deltaTime;
            currentTimeToTravel += maxTimeToTravel / maxTimeToCharge * Time.deltaTime;
            currentMotorForceMultiplier = Mathf.Min(currentMotorForceMultiplier, 1f);
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
