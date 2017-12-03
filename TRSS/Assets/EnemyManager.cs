using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    private GameObject[] lampPosts;

    public int EnemyCount = 100;

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
        enemies = new GameObject[lampPosts.Length];

        int j = 0;
        for(int i = 0; i < lampPosts.Length; i++)
        {
            if(lampPosts[i] != null)
            {
                GameObject enemy = Resources.Load("Enemy") as GameObject;
                enemies[j] = (GameObject)Instantiate(enemy, new Vector3(lampPosts[i].transform.position.x, .1f, lampPosts[i].transform.position.z), Quaternion.identity);
                enemies[j].GetComponent<FollowScript>().SetLampPosts(lampPosts);
                j++;
            }
        }
    }
}
