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
        void Start()
        {

            //Debugger.Print("Called");

        }

        void Populate()
        {

            int rand;

            for (int x = 0; x < gObjs.Length; x++) {

                for (int y = 0; y < gObjs[x].Length; y++) {

                    if (gObjs[x][y] == null) {

                        rand = Random.Range(0, 10);
                        if (rand >= 5)
                        {

                            Debugger.Print("House Created");
                            Instantiate(house, new Vector3((x * 10) - 70, 1.97f, (y * 10) - 45), Quaternion.identity);

                        }

                    }

                }

            }

        }

        // Update is called once per frame
        void Update()
        {

            if (aManager != null & gObjs == null && aManager.arena != null) {

                gObjs = aManager.arena;
                Debug.Log(gObjs);
                Populate();

            }

        }
    }
}
