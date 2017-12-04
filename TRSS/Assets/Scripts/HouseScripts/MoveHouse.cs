using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TRSS
{
    public class MoveHouse : MonoBehaviour
    {

        public Text score_text;
        public TimerUpdate timer;

        int score = 0;

        // Use this for initialization
        void Start()
        {
            score_text = GameObject.Find("Score").GetComponent<Text>();
            timer = GameObject.Find("Timer").GetComponent<TimerUpdate>();

        }

        void DeleteHouse()
        {

            Destroy(gameObject);

        }

        void UpdateScore()
        {
            score++;
            score_text.text = "Deliveries: " + score;
        }

        void UpdateTimer()
        {
            timer.AddSubTime(3);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "MailTruck(Clone)")
            {

                DeleteHouse();
                UpdateScore();
                UpdateTimer();
            }

        }

        // Update is called once per frame
        void Update()
        {



        }
    }
}
