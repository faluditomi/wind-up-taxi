using UnityEngine;

public class CarMovementController : MonoBehaviour
{
    private float steeringInput = 0f;
    private float speedInput = 0f;
    [SerializeField] float motorTorque = 2000;
    [SerializeField] float brakeTorque = 2000;
    [SerializeField] float maxSpeed = 20;
    [SerializeField] float steeringRange = 30;
    [SerializeField] float steeringRangeAtMaxSpeed = 10;
    [SerializeField] float centreOfGravityOffset = -1f;

    WheelController[] wheels;
    Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        // Adjust center of mass vertically, to help prevent the car from rolling
        rigidBody.centerOfMass += Vector3.up * centreOfGravityOffset;

        // Find all child GameObjects that have the WheelController script attached
        wheels = GetComponentsInChildren<WheelController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate current speed in relation to the forward direction of the car
        // (this returns a negative number when traveling backwards)
        float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.linearVelocity);


        // Calculate how close the car is to top speed
        // as a number from zero to one
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);

        // Use that to calculate how much torque is available 
        // (zero torque at top speed)
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);

        // …and to calculate how much to steer 
        // (the car steers more gently at top speed)
        float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        // Check whether the user input is in the same direction 
        // as the car's velocity
        bool isAccelerating = Mathf.Sign(speedInput) == Mathf.Sign(forwardSpeed);

        foreach (var wheel in wheels)
        {
            // Apply steering to Wheel colliders that have "Steerable" enabled
            if (wheel.steerable)
            {
                wheel.WheelCollider.steerAngle = steeringInput * currentSteerRange;
            }
            
            if (isAccelerating)
            {
                // Apply torque to Wheel colliders that have "Motorized" enabled
                if (wheel.motorized)
                {
                    wheel.WheelCollider.motorTorque = speedInput * currentMotorTorque;
                }
                wheel.WheelCollider.brakeTorque = 0;
            }
            else
            {
                // If the user is trying to go in the opposite direction
                // apply brakes to all wheels
                wheel.WheelCollider.brakeTorque = Mathf.Abs(speedInput) * brakeTorque;
                wheel.WheelCollider.motorTorque = 0;
            }
        }
    }

    public void SetSteeringInput(float value)
    {
        steeringInput = value;
    }

    public void SetSpeedingInput(float value)
    {
        speedInput = value;
    }
}
