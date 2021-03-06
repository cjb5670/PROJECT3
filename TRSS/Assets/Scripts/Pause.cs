﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

    public GameObject ui;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetButtonDown("Pause"))
        {
            TogglePauseMenu();
        }
        else if(Input.GetButtonDown("B") && ui.GetComponent<Canvas>().enabled)
        {
            ui.GetComponent<Canvas>().enabled = false;
            Time.timeScale = 1;
        }
	}

    public void TogglePauseMenu()
    {
        if(ui.GetComponent<Canvas>().enabled)
        {
            ui.GetComponent<Canvas>().enabled = false;
            Time.timeScale = 1;
        }
        else
        {
            ui.GetComponent<Canvas>().enabled = true;
            Time.timeScale = 0;
        }
    }
}
