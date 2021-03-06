﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TRSS
{
    public class CreateHouses : MonoBehaviour
    {

        public ArenaManager aManager;

        GameObject[][] gObjs;

        public List<GameObject> houses = new List<GameObject>();
        public bool curHouse = false;

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
            for (int x = 0; x < gObjs.Length; x++) {

                for (int y = 0; y < gObjs[x].Length; y++) {

                    if (gObjs[x][y] == null) {

                        rand = Random.Range(0, 10);
                        if (rand >= 5)
                        {
                            if(index != maxHouseCount)
                            {
                                Debugger.Print("House Created");
                                houses.Add((GameObject)Instantiate(house, new Vector3((x * 10) - 70, 1.97f, (y * 10) - 45), Quaternion.identity));
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
            if (houses.Count != 0) {
                int randomHouse = Random.Range(0, houses.Count);
                while (houses[randomHouse] == null)
                {
                    randomHouse = Random.Range(0, houses.Count);
                }
                curHouse = true;
                this.currentHouse = randomHouse;
                Debugger.Print("New House Set");
                houses[randomHouse].GetComponent<MoveHouse>().SetActiveHouse();
            }
            else
            {
                SceneManager.LoadScene(4);
            }
        }

        // Update is called once per frame
        void Update()
        {

            if (aManager != null & gObjs == null && aManager.arena != null) {

                gObjs = aManager.arena;
                this.maxHouseCount = 50;// aManager.grid_x * aManager.grid_y / 2;
                Debug.Log(gObjs);
                Populate();

            }

            if(curHouse == false)
            {
                SelectNewDelivery();
            }
        }
    }
}
