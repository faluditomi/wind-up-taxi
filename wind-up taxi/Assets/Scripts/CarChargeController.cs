using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Car : MonoBehaviour
{
    private CarStateController carStateController;
    private CarMovementController carMovementController;
    private CinemachineBasicMultiChannelPerlin shake;
    private Arrow arrowController;

    private Rigidbody myRigidBody;

    private Coroutine chargeCoroutine;
    private Coroutine timerCoroutine;
    private Coroutine moveCoroutine;

    private float currentMotorForceMultiplier;
    private float currentTimeToTravel;
    [SerializeField] float maxTimeToTravel = 2f;
    [SerializeField] float maxTimeToCharge = 8f;
    [SerializeField] float maxTimeToOvercharge = 2f;
    [SerializeField] float maxCamShake = 1f;
    [SerializeField] float minTimeToTravel = 0.75f;
    [SerializeField] float minMotorForceMultiplier = 20f;

    [SerializeField] bool isInCarMode = false;

    private void Awake()
    {
        carStateController = GetComponent<CarStateController>();
        carMovementController = GetComponent<CarMovementController>();
        myRigidBody = GetComponent<Rigidbody>();
        arrowController = FindAnyObjectByType<Arrow>();
        shake = FindFirstObjectByType<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start()
    {
        currentMotorForceMultiplier = minMotorForceMultiplier;
        currentTimeToTravel = minTimeToTravel;
        shake.enabled = false;    
    }

    // private void FixedUpdate()
    // {
    //     if(carStateController.GetState() == CarStateController.CarState.Idle)
    //     {
    //         myRigidBody.isKinematic = true;
    //     }
    //     else
    //     {
    //         myRigidBody.isKinematic = false;
    //     }
    // }

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

                shake.enabled = false;

                moveCoroutine = StartCoroutine(MoveBehaviour());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Destination") && other.transform == arrowController.GetCurrentDestination())
        {
            if(!arrowController.GetIsPassenger())
            {
                arrowController.PickupPassenger();
            }
            else
            {
                arrowController.DeliverPassenger();
            }
        }
    }

    public void Reset()
    {
        carMovementController.modifyCurrentMotorForce(0f);
        currentMotorForceMultiplier = minMotorForceMultiplier / 100f;
        currentTimeToTravel = minTimeToTravel;
        shake.AmplitudeGain = 0f;
        shake.enabled = false;

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

        shake.enabled = true;

        while(carStateController.GetState() == CarStateController.CarState.Charging)
        {
            //camera zoom

            shake.AmplitudeGain += maxCamShake / maxTimeToCharge * Time.deltaTime;
            shake.AmplitudeGain = Mathf.Min(shake.AmplitudeGain, maxCamShake);

            currentMotorForceMultiplier += (1f - (minMotorForceMultiplier / 100f)) / maxTimeToCharge * Time.deltaTime;
            currentMotorForceMultiplier = Mathf.Min(currentMotorForceMultiplier, 1f);

            currentTimeToTravel += (maxTimeToTravel - minTimeToTravel) / maxTimeToCharge * Time.deltaTime;
            currentTimeToTravel += maxTimeToTravel / maxTimeToCharge * Time.deltaTime;
            currentTimeToTravel = Mathf.Min(currentTimeToTravel, maxTimeToTravel);

            yield return null;
        }

        timerCoroutine = StartCoroutine(carStateController.TimedSetStateBehaviour(maxTimeToOvercharge, CarStateController.CarState.Busted));

        while(carStateController.GetState() == CarStateController.CarState.Overcharging)
        {
            //camera shake

            yield return null;
        }

        if(carStateController.GetState() == CarStateController.CarState.Busted)
        {
            Reset();
        }
    }
}
