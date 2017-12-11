using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour {

    public RectTransform health;

    public int mailtruck_health = 1000;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ApplyDamage(int damage)
    {
        mailtruck_health -= damage;
        Debug.Log(mailtruck_health);

        health.sizeDelta = new Vector2(mailtruck_health, health.sizeDelta.y);
        float damage_dealt = damage * .5f;
        health.position = new Vector2(health.position.x + damage_dealt, health.position.y);

        if(mailtruck_health <= 0)
        {
            SceneManager.LoadScene(4);
        }
    }
}
