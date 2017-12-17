using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	
	public Vector3 thisPosition;
	public float height;
	public GameObject target;

	// Use this for initialization
	void Start ()
	{
		thisPosition = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
        if(target != null)
        {
            // sets thisPosition to the location of the target + height
            thisPosition.x = target.transform.position.x;
            thisPosition.y = height;
            thisPosition.z = target.transform.position.z;

            // sets the actual position of the camera to stored value
            gameObject.transform.position = thisPosition;
        }
	}

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }
}
