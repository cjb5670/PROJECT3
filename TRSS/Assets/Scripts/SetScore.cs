using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetScore : MonoBehaviour {

    public Text score_text = null;
    // Use this for initialization
    void Start()
    {

        score_text.text = "You Delivered " + ScoreCounter.Score + " mail!";
        ScoreCounter.Score = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
