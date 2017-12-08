using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TRSS
{
    public class MoveHouse : MonoBehaviour
    {

        public bool target = false;

        public Text score_text;
        public TimerUpdate timer;

        public ArenaManager aManager;
        public CreateHouses cHouse;

        int score = 0;

        // Use this for initialization
        void Start()
        {
            score_text = GameObject.Find("Score").GetComponent<Text>();
            timer = GameObject.Find("Timer").GetComponent<TimerUpdate>();
            aManager = Object.FindObjectOfType<ArenaManager>();
            cHouse = aManager.GetComponent<CreateHouses>();
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

        public void SetActiveHouse()
        {

            target = true;
            Debug.Log(target);
            Renderer rend = GetComponent<Renderer>();
            rend.material.shader = Shader.Find("Specular");
            //rend.material.SetColor("_Target", Color.red);

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "MailTruck(Clone)")
            {
                DeleteHouse();
                if (target == true)
                {
                    Debug.Log("Reached");

                    UpdateScore();

                    cHouse.SelectNewDelivery();
                }
                UpdateTimer();

            }

        }

        // Update is called once per frame
        void Update()
        {



        }
    }
}
