using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TRSS.Input;

public class MailTruckMovement : TRSS.Input.ControllerInput {

    #region Data Members

    #region Trigger Keys.

    const string DPAD_X = "Digital Pad X";
    const string DPAD_Y = "Digital Pad Y";
    const string LS_Y = "Left Stick Y";
    const string RS_X = "Right Stick X";
    const string RTRIG = "Right Trigger";
    const string LTRIG = "Left Trigger";

    #endregion

    #region Fields.

    /// <summary>
    /// Force applied for acceleration.
    /// </summary>
    private float speedForce = 2500f;

    /// <summary>
    /// Dampening angle for steering.
    /// </summary>
    private float steerAngle = 45f;

    /// <summary>
    /// Force applied for braking.
    /// </summary>
    private float brakeForce = 5000f;
    private bool isBraking = false;

    /// <summary>
    /// Movement axis value is applied every frame.
    /// </summary>
    private Vector2 movementAxis = Vector2.zero;

    public WheelCollider FR_L, FR_R, BK_L, BK_R;
    public Rigidbody car_rb;
    public float MAX_SPEED = 1.0f;    

    #endregion

    #region Properties.

    public float ForwardForce
    {
        get { return this.speedForce * movementAxis.y; }
    }

    public float Steering
    {
        get { return this.steerAngle * movementAxis.x; }
    }

    #endregion

    #endregion
    
    #region Trigger Initialization.

    /// <summary>
    /// Create input scheme triggers here.
    /// </summary>
    public override void InitializeTriggers()
    {
        this.Scheme.AddTrigger(DPAD_X, Controls.DPAD_X, ControlResponse.AXIS);
        this.Scheme.AddTrigger(DPAD_Y, Controls.DPAD_Y, ControlResponse.AXIS);
        this.Scheme.AddTrigger(LS_Y, Controls.LS_Y, ControlResponse.AXIS);
        this.Scheme.AddTrigger(RS_X, Controls.RS_X, ControlResponse.AXIS);
        this.Scheme.AddTrigger(RTRIG, Controls.RTRIG, ControlResponse.AXIS);
        this.Scheme.AddTrigger(LTRIG, Controls.LTRIG, ControlResponse.AXIS);
    }

    #endregion
    
    #region Handle Input.

    /// <summary>
    /// Handle input scheme triggers here.
    /// </summary>
    public override void HandleInput()
    {
        // Reset cycle flags.
        this.isBraking = false;
        this.movementAxis = Vector2.zero;

        List<Trigger> throttle = new List<Trigger>();
        throttle.Add(this.Scheme.GetTrigger(DPAD_Y));
        throttle.Add(this.Scheme.GetTrigger(LS_Y));
        throttle.Add(this.Scheme.GetTrigger(RTRIG));

        float offset = 0.0f;
        for (int i = 0; i < throttle.Count; i++)
        {
            int sign = 1;

            if(i == throttle.Count - 1) { sign = -1; }

            float temp = throttle[i].GetAxis() * sign;
            if (Mathf.Abs(temp) > Mathf.Abs(offset))
            {
                offset = temp;
            }
        }
        
        this.movementAxis.y = -offset;

        List<Trigger> steering = new List<Trigger>();
        steering.Add(this.Scheme.GetTrigger(DPAD_X));
        steering.Add(this.Scheme.GetTrigger(RS_X));

        float accel = 0.0f;
        for (int j = 0; j < steering.Count; j++)
        {
            float temp = steering[j].GetAxis();
            if (Mathf.Abs(temp) > Mathf.Abs(accel))
            {
                accel = temp;
            }
        }

        // Gets average from all possible inputs.
        this.movementAxis.x = accel;

        Trigger braking = this.Scheme.GetTrigger(LTRIG);
        this.isBraking = (braking.GetAxis() > 0.0f);        
    }

    #endregion
    
    /**
     * FixedUpdate is called each frame. During the fixed update of the Movement script,
     * we want to apply a forward torque on the car's wheels and a steering depending
     * on user input.
     **/
    void FixedUpdate () {
        
        if (!this.isBraking)
        {
            this.ApplyForwardTorque(this.ForwardForce);
            GameObject.Find("BrakeLightL").GetComponent<Light>().enabled = false;
            GameObject.Find("BrakeLightR").GetComponent<Light>().enabled = false;
        }
        else
        {
            this.ApplyBrake();
            GameObject.Find("BrakeLightL").GetComponent<Light>().enabled = true;
            GameObject.Find("BrakeLightR").GetComponent<Light>().enabled = true;
        }
        
        // float forwardForce = speedForce * Input.GetAxis("Vertical");
        // float steering = steerAngle* Input.GetAxis("Horizontal");
        
        // Apply the steering angle to the vehicle
        ApplySteeringTorque(this.Steering);
        LimitVelocity();
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

        FR_L.motorTorque = torqueForce;
        FR_R.motorTorque = torqueForce;
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

    void LimitVelocity()
    {
        //Debug.Log(car_rb.velocity.magnitude);
        if (car_rb.velocity.magnitude > MAX_SPEED)
        {
            // Add brake torque to rear wheels
            BK_L.brakeTorque = brakeForce;
            BK_R.brakeTorque = brakeForce;

            // Add brake torque to front wheels
            FR_L.brakeTorque = brakeForce;
            FR_R.brakeTorque = brakeForce;
        }
    }
}
