using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUpdate : MonoBehaviour {

    int time_left = 10;

    private float time = 0.0f;
    public float interpolationPeriod = 1f;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        time += Time.deltaTime;

        if(time > interpolationPeriod)
        {
            time -= interpolationPeriod;

           

            time_left -= 1;

            if (time_left == 0)
            {
                Application.Quit();
            }
        }
        UpdateTimeRemaining();
    }

    public void AddSubTime(int time)
    {
        time_left += time;
    }

    void UpdateTimeRemaining()
    {
        gameObject.GetComponent<Text>().text = "Time Remaining: " + time_left;
    }
}
