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
            cHouse.houses.Remove(gameObject);

        }

        void UpdateScore()
        {
            cHouse.score++;
            ScoreCounter.Score = cHouse.score;
            score_text.text = "Deliveries: " + cHouse.score;
        }

        void UpdateTimer()
        {
            timer.AddSubTime(7);
        }

        public void SetActiveHouse()
        {

            target = true;
            Debug.Log(target);
            Renderer rend = GetComponent<Renderer>();
            //rend.material.shader = Shader.Find("Specular");
            gameObject.GetComponentInChildren<ParticleSystem>().enableEmission = true;
            //rend.material.SetColor("_Target", Color.red);

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "MailTruck(Clone)")
            {
                
                if (target == true)
                {                    
                    Debug.Log("Reached");

                    UpdateScore();
                    UpdateTimer();
                    
                    other.gameObject.GetComponent<Health>().ApplyDamage(-10);
                    DeleteHouse();
                }
                

            }

        }

        // Update is called once per frame
        void Update()
        {



        }
    }
}
