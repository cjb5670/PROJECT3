using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHouse : MonoBehaviour {

    // Use this for initialization
    void Start()
    {



    }

    void ChangePosition() {

        //ground size is 100:100 )||( house size is 5:5

        Vector3 newPosition;
        float newX;
        float newZ;

        newX = Random.Range(0, 95);
        newZ = Random.Range(0, 95);

        newX -= 45;
        newZ -= 45;

        newPosition = new Vector3(newX, transform.position.y, newZ);
        transform.position = newPosition;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "MailTruck")
        {

            ChangePosition();

        }

    }

    // Update is called once per frame
    void Update () {

        

	}
}
