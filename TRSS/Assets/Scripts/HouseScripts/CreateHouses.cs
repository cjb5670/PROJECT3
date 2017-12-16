using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TRSS
{
    public class CreateHouses : MonoBehaviour
    {

        public ArenaManager aManager;

        GameObject[][] gObjs;

        GameObject[] houses;

        public GameObject house;
        int maxHouseCount;
        int currentHouse = 0;
        public int score = 0;

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

            Debug.Log("Called");

            SelectNewDelivery();

        }

        public void SelectNewDelivery()
        {

            if (houses.Length != 0) {
                int randomHouse = Random.Range(0, maxHouseCount);
                while (houses[randomHouse] == null)
                {
                    randomHouse = Random.Range(0, maxHouseCount);
                }
                this.currentHouse = randomHouse;
                Debugger.Print("New House Set");
                houses[randomHouse].GetComponent<MoveHouse>().SetActiveHouse();
            }
            else
            {
                SceneManager.LoadScene(6);
            }
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

            }

            if(houses[currentHouse] == null && houses.Length > 0 && houses != null)
            {
                SelectNewDelivery();
            }
        }
    }
}
