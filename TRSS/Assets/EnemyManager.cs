using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    private GameObject[] lampPosts;

    public int EnemyCount = 10;

    GameObject[] enemies;

    int dist = 10;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetLampPostArray(GameObject[] lampPosts)
    {
        this.lampPosts = lampPosts;
    }

    public void Initialize()
    {
        enemies = new GameObject[EnemyCount];

        for (int i = 0; i < EnemyCount; i++)
        {
            GameObject enemy = Resources.Load("Enemy") as GameObject;
            enemies[i] = (GameObject)Instantiate(enemy, new Vector3(dist * i, .0f, dist), Quaternion.identity);
            enemies[i].GetComponent<FollowScript>().SetLampPosts(lampPosts);
        }
    }
}
