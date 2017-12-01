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

        public GameObject house;

        // Use this for initialization
        void Start()
        { 

        }

        void DestroyHouses()
        {

            Destroy(gameObject);

            //ground size is 100:100 )||( house size is 5:5
            /*
            int rand;

            for (int x = 0; x < gObjs.Length; x++)
            {

                for (int y = 0; y < gObjs[x].Length; y++)
                {

                    if (gObjs[x][y] == null)
                    {

                        rand = Random.Range(0, 10);
                        if (rand >= 5) {

                            Debugger.Print("House Created");
                            Instantiate(house, new Vector3((x*10)-45, 1.97f, (y*10)-45), Quaternion.identity);
                        }

                    }

                }

            }
            */

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
                DestroyHouses();
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
