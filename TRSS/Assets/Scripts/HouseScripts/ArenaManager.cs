using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour {

    int houses_in_x = 10;
    int houses_in_y = 10;

    GameObject[][] arena;

    // Use this for initialization
    void Start () {
        arena = new GameObject[houses_in_x][];
        for (int x = 0; x < houses_in_x; x++)
        {
            arena[x] = new GameObject[houses_in_y];
            for(int y = 0; y < houses_in_y; y++)
            {
                GameObject road = Resources.Load("Road") as GameObject;
                arena[x][y] = (GameObject)Instantiate(road, new Vector3(x * 10 - 45,.5f, y * 10 - 45), Quaternion.identity);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
