using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailTruckMovement : MonoBehaviour {

    float speedForce = 1500f;
    float steerAngle = 65f;
    float brakeForce = 1000000f;

    public WheelCollider FR_L, FR_R, BK_L, BK_R;

	// Use this for initialization
	void Start () {
        

    }

    private void Update()
    {
        // If we neeed to do get button down/up set a bool here then use fixed update.
        


    }

    /**
     * FixedUpdate is called each frame. During the fixed update of the Movement script,
     * we want to apply a forward torque on the car's wheels and a steering depending
     * on user input.
     **/
    void FixedUpdate () {

        float forwardForce = speedForce * Input.GetAxis("Vertical");
        float steering = steerAngle* Input.GetAxis("Horizontal");

        // Apply forward torque to the car.
        if (Input.GetButton("Vertical"))
        {
            ApplyForwardTorque(forwardForce);
        } else
        {
            // If not accelerating, apply the break.
            ApplyBrake();
        }

        // Apply the steering angle to the vehicle
        ApplySteeringTorque(steering);
    }

    /**
     * Apply forward torque will apply a force to the rotation
     * of the rear wheels of the vehicle. If the force is negative
     * then it will move the vehicle in reverse.
     **/
    void ApplyForwardTorque(float torqueForce)
    {
        // Stop braking if we are currently braking
        BK_L.brakeTorque = 0;
        BK_R.brakeTorque = 0;
        FR_L.brakeTorque = 0;
        FR_R.brakeTorque = 0;

        // Apply the torque force to rear wheels
        BK_L.motorTorque = torqueForce;
        BK_R.motorTorque = torqueForce;

        //Apply the torque force to front wheels

        //FR_L.motorTorque = torqueForce;
        //FR_R.motorTorque = torqueForce;
    }

    /**
     * Apply the brake force to the vehicle.
     **/
    void ApplyBrake()
    {
        // Remove any torque currently being applied to the wheels
        BK_L.motorTorque = 0;
        BK_R.motorTorque = 0;

        // Add brake torque to rear wheels
        BK_L.brakeTorque = brakeForce;
        BK_R.brakeTorque = brakeForce;

        // Add brake torque to front wheels
        FR_L.brakeTorque = brakeForce;
        FR_R.brakeTorque = brakeForce;
    }

    /**
     * Apply the steering angle to the vehicle.
     **/
    void ApplySteeringTorque(float steeringAngle)
    {
        FR_L.steerAngle = steeringAngle;
        FR_R.steerAngle = steeringAngle;
    }
}
