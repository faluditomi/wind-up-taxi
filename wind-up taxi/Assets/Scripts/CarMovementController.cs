using System;
using UnityEngine;

public class CarMovementController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private float currentMotorForce;
    [SerializeField] private float maxMotorForce, breakForce, maxSteerAngle, antiRollForce;
    
    private bool isBreaking;

    
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;
    [SerializeField] private Transform CamFollowPoint;

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        ApplyAntiRoll(frontLeftWheelCollider, frontRightWheelCollider);
        ApplyAntiRoll(rearLeftWheelCollider, rearRightWheelCollider);
        CamFollowPoint.rotation = new Quaternion(CamFollowPoint.rotation.x, transform.rotation.y, CamFollowPoint.rotation.z, CamFollowPoint.rotation.w);
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

    public void ApplyAntiRoll(WheelCollider leftWheel, WheelCollider rightWheel)
    {
        WheelHit hitLeft, hitRight;
        float travelLeft = 1f, travelRight = 1f;

        if (leftWheel.GetGroundHit(out hitLeft))
            travelLeft = (-leftWheel.transform.InverseTransformPoint(hitLeft.point).y - leftWheel.radius) / leftWheel.suspensionDistance;
        if (rightWheel.GetGroundHit(out hitRight))
            travelRight = (-rightWheel.transform.InverseTransformPoint(hitRight.point).y - rightWheel.radius) / rightWheel.suspensionDistance;

        // Calculate the anti-roll force
        float force = (travelLeft - travelRight) * antiRollForce;

        // Limit the force to avoid jerking
        force = Mathf.Clamp(force, -antiRollForce, antiRollForce);

        Rigidbody rb = GetComponent<Rigidbody>();

        // Apply the forces to the wheels
        if (leftWheel.isGrounded)
            rb.AddForceAtPosition(leftWheel.transform.up * -force, leftWheel.transform.position);
        if (rightWheel.isGrounded)
            rb.AddForceAtPosition(rightWheel.transform.up * force, rightWheel.transform.position);
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
