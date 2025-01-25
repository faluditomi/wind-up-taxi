using System;
using UnityEngine;

public class CarMovementController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;

    // Settings
    [SerializeField] private float maxMotorForce, breakForce, maxSteerAngle, antiRollForce;

    private float currentMotorForce;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        ApplyAntiRoll();
    }

    public void setHorizontalInput(float value)
    {
        horizontalInput = value;
    }

    public void setVerticalInput(float value)
    {
        verticalInput = value;
    }

    public void setBreakingInput(bool value)
    {
        isBreaking = value;
    }

    public void modifyCurrentMotorForce(float value)
    {
        currentMotorForce = maxMotorForce * value;
    }

    public void ApplyAntiRoll()
    {
        WheelHit hitLeft, hitRight;
        float travelLeft = 1f, travelRight = 1f;

        if (rearLeftWheelCollider.GetGroundHit(out hitLeft))
            travelLeft = (-rearLeftWheelCollider.transform.InverseTransformPoint(hitLeft.point).y - rearLeftWheelCollider.radius) / rearLeftWheelCollider.suspensionDistance;
        if (rearRightWheelCollider.GetGroundHit(out hitRight))
            travelRight = (-rearRightWheelCollider.transform.InverseTransformPoint(hitRight.point).y - rearRightWheelCollider.radius) / rearRightWheelCollider.suspensionDistance;

        float force = (travelLeft - travelRight) * antiRollForce;
        if (rearLeftWheelCollider.isGrounded)
            GetComponent<Rigidbody>().AddForceAtPosition(rearLeftWheelCollider.transform.up * -force, rearLeftWheelCollider.transform.position);
        if (rearRightWheelCollider.isGrounded)
            GetComponent<Rigidbody>().AddForceAtPosition(rearRightWheelCollider.transform.up * force, rearRightWheelCollider.transform.position);
    }

    private void HandleMotor() {
        frontLeftWheelCollider.motorTorque = verticalInput * currentMotorForce;
        frontRightWheelCollider.motorTorque = verticalInput * currentMotorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking() {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering() {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels() {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform) {
        Vector3 pos;
        Quaternion rot; 
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
