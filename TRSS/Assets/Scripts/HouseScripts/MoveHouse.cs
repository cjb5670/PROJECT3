using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveHouse : MonoBehaviour {

    public Text score_text;

    int score = 0;

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

    void UpdateScore()
    {
        score++;
        score_text.text = "Deliveries: " + score;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "MailTruck")
        {

            ChangePosition();
            UpdateScore();
        }

    }

    // Update is called once per frame
    void Update () {

        

	}
}
