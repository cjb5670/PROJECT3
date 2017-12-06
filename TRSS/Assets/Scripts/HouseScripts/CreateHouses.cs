using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TRSS
{
    public class CreateHouses : MonoBehaviour
    {

        public ArenaManager aManager;

        GameObject[][] gObjs;

        GameObject[] houses;

        public GameObject house;
        int maxHouseCount;

        // Use this for initialization
        void Start()
        {

            //Debugger.Print("Called");

        }

        void Populate()
        {

            int rand;
            int index = 0;
            houses = new GameObject[maxHouseCount];
            for (int x = 0; x < gObjs.Length; x++) {

                for (int y = 0; y < gObjs[x].Length; y++) {

                    if (gObjs[x][y] == null) {

                        rand = Random.Range(0, 10);
                        if (rand >= 5)
                        {
                            if(index != maxHouseCount)
                            {
                                Debugger.Print("House Created");
                                houses[index] = (GameObject)Instantiate(house, new Vector3((x * 10) - 70, 1.97f, (y * 10) - 45), Quaternion.identity);
                                index++;
                            }
                        }

                    }

                }

            }

        }

        void SelectNewDelivery()
        {
            int randomHouse = Random.Range(0, maxHouseCount);
            while(houses[randomHouse] == null)
            {
                randomHouse = Random.Range(0, maxHouseCount);
            }
            houses[randomHouse].GetComponent<MoveHouse>().SetActiveHouse();
            houses[randomHouse] = null;
        }

        // Update is called once per frame
        void Update()
        {

            if (aManager != null & gObjs == null && aManager.arena != null) {

                houses = new GameObject[maxHouseCount];

                gObjs = aManager.arena;
                this.maxHouseCount = aManager.grid_x * aManager.grid_y / 2;
                Debug.Log(gObjs);
                Populate();

                SelectNewDelivery();

            }

        }
    }
}
