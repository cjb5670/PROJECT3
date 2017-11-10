using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailTruckMovement : MonoBehaviour {

    float speedForce = 50f;
    float torqueForce = -2f;


    Rigidbody2D mailtruck_rb = null;

	// Use this for initialization
	void Start () {
        mailtruck_rb = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        // If we neeed to do get button down/up set a bool here then use fixed update.
        
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (Input.GetButton("Accelerate"))
        {
            mailtruck_rb.AddForce(transform.up * speedForce);
        }
        //Vector3 torque = new Vector3(0, Input.GetAxis("Horizontal") * torqueForce, 0);
        mailtruck_rb.AddTorque(Input.GetAxis("Horizontal") * torqueForce);
    }

    Vector3 ForwardVelocity()
    {
        return Vector3.zero;
    }
}
