using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TRSS
{
    public class CreateHouses : MonoBehaviour
    {

        public ArenaManager aManager;
        GameObject[][] gObjs;

        public GameObject house;

        // Use this for initialization
        void Start(ArenaManager aMan)
        {

            aManager = aMan;
            gObjs = aManager.arena;
            Populate();

        }

        void Populate()
        {

            //ground size is 100:100 )||( house size is 5:5

            int rand;

            for (int x = 0; x < gObjs.Length; x++)
            {

                for (int y = 0; y < gObjs[x].Length; y++)
                {

                    if (gObjs[x][y] == null)
                    {

                        rand = Random.Range(0, 10);
                        if (rand >= 5)
                        {

                            Debugger.Print("House Created");
                            Instantiate(house, new Vector3((x * 10) - 45, 1.97f, (y * 10) - 45), Quaternion.identity);

                        }

                    }

                }

            }

        }

        // Update is called once per frame
        void Update()
        {



        }
    }
}
