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

    private float currentMaxMotorForceMultiplier = 0f;
    private float currentTimeToTravel = 0f;
    [SerializeField] float maxTimeToTravel = 2f;
    [SerializeField] float maxTimeToCharge = 8f;
    [SerializeField] float maxTimeToOvercharge = 2f;

    [SerializeField] bool isInCarMode = false;

    private void Awake()
    {
        carStateController = new CarStateController();
        carMovementController = GetComponent<CarMovementController>();
        myRigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        GetInput();
    }

    private void GetInput() {
        if(isInCarMode)
        {
            carMovementController.setHorizontalInput(Input.GetAxis("Horizontal"));
            carMovementController.setVerticalInput(Input.GetAxis("Vertical"));
            carMovementController.setBreakingInput(Input.GetKey(KeyCode.Space));
        }
        else
        {
            if(carStateController.GetState() == CarStateController.CarState.Moving)
            {
                carMovementController.setVerticalInput(Input.GetAxis("Vertical"));
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
        currentMaxMotorForceMultiplier = 0f;
        currentTimeToTravel = 0f;

        if(carStateController.GetState() == CarStateController.CarState.Busted)
        {
            //GameController.GameOver();
        }
    }

    private IEnumerator MoveBehaviour()
    {
        carStateController.SetState(CarStateController.CarState.Moving);
        carMovementController.modifyCurrentMotorForce(currentMaxMotorForceMultiplier);
        carMovementController.setVerticalInput(1f);
        
        yield return new WaitForSeconds(currentTimeToTravel);

        carMovementController.modifyCurrentMotorForce(0);
        carMovementController.setVerticalInput(0);

        yield return new WaitUntil(() => myRigidBody.linearVelocity.sqrMagnitude <= 0.1f);

        carStateController.SetState(CarStateController.CarState.Idle);

        myRigidBody.linearVelocity = Vector3.zero;
        myRigidBody.angularVelocity = Vector3.zero;
        moveCoroutine = null;
    }

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

            currentMaxMotorForceMultiplier += 1f / maxTimeToCharge * Time.deltaTime;
            currentTimeToTravel += maxTimeToTravel / maxTimeToCharge * Time.deltaTime;
            currentMaxMotorForceMultiplier = Mathf.Min(currentMaxMotorForceMultiplier, 1f);
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
