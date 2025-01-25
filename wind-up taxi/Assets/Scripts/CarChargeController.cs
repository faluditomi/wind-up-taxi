using System.Collections;
using UnityEngine;

public class Car : MonoBehaviour
{
    private CarStateController carStateController = new CarStateController();

    private Coroutine chargeCoroutine;
    private Coroutine timerCoroutine;

    private float currentDistanceToTravel = 0f;
    private float currentTimeToTravel = 0f;
    [SerializeField] float maxDistanceToTravel = 5f;
    [SerializeField] float maxTimeToTravel = 2f;
    [SerializeField] float maxTimeToCharge = 8f;
    [SerializeField] float maxTimeToOvercharge = 2f;

    //called when charge button is let go of
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
