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

        public ArenaManager aManager;
        GameObject[][] gObjs;

        // Use this for initialization
        void Start()
        {



        }

        void ChangePosition()
        {

            //ground size is 100:100 )||( house size is 5:5

            for (int x = 0; x < gObjs.Length; x++)
            {

                for (int y = 0; y < gObjs[x].Length; y++)
                {

                    if (gObjs[x][y] == null)
                    {



                    }

                }

            }

            /*Vector3 newPosition;
            float newX;
            float newZ;

            newX = Random.Range(0, 95);
            newZ = Random.Range(0, 95);

            newX -= 45;
            newZ -= 45;

            newPosition = new Vector3(newX, transform.position.y, newZ);
            transform.position = newPosition;*/

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
            if (other.gameObject.name == "MailTruck")
            {

                ChangePosition();
                UpdateScore();
                UpdateTimer();
            }

        }

        // Update is called once per frame
        void Update()
        {

            if (aManager != null && gObjs == null && aManager.arena != null) {

                gObjs = aManager.arena;

            }

        }
    }
}
