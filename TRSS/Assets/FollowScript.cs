using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowScript : MonoBehaviour {

    public float range;
    public float speed;

    public GameObject mailTruck;

    private GameObject lampPost;

    private GameObject[] lampPosts;

    bool searchNewTarget = false;

    bool keepMoving = true;

	// Use this for initialization
	void Start () {
        mailTruck = GameObject.Find("MailTruck");
		
	}
	
	// Update is called once per frame
	void Update () {

        if(keepMoving && (mailTruck.transform.position - transform.position).magnitude > 1 && (mailTruck.transform.position - transform.position).magnitude < range)
        {
            searchNewTarget = false;
            transform.LookAt(mailTruck.transform.position);
            //transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
            transform.Translate(Vector3.forward * speed);
        }
        else if(keepMoving)
        {
            if(!searchNewTarget)
            {
                SelectNewTarget();
                searchNewTarget = true;
            }
            if((lampPost.transform.position - transform.position).magnitude > 1)
            {
                transform.LookAt(lampPost.transform.position);
                transform.Translate(Vector3.forward * speed);
            }
        }
		
	}

    public void SelectNewTarget()
    {
        foreach(GameObject lamp in lampPosts)
        {
            if(lamp != null)
            {
                if (lampPost == null)
                {
                    lampPost = lamp;
                }
                else if ((lamp.transform.position - transform.position).magnitude < (lampPost.transform.position - transform.position).magnitude)
                {
                    lampPost = lamp;
                }
            }
        }
    }

    public void SetLampPosts(GameObject[] lamps)
    {
        this.lampPosts = lamps;
    }

    Coroutine damageCO;

    private void OnTriggerStay(Collider col)
    {
        if(col.gameObject.name == "MailTruck")
        {
            //keepMoving = false;
            if(damageCO == null)
            {
                damageCO = StartCoroutine(DamageCO(col.gameObject));
            }
            
            
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "MailTruck")
        {
            if(damageCO != null)
            {
                StopCoroutine(damageCO);
                damageCO = null;
            }
        }
    }

    IEnumerator DamageCO(GameObject obj)
    {
        
        while(true)
        {
            obj.GetComponent<Health>().ApplyDamage(10);
            yield return new WaitForSeconds(1f);
        }
        //keepMoving = true;
    }
}
