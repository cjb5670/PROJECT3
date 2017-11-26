using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTruckBounce : MonoBehaviour
{
	public float frequency;
	public float magnitude;
	private Vector3 newHeight;
	

	// Use this for initialization
	void Start ()
	{
		newHeight = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update ()
	{
		newHeight.y = Mathf.Sin(Time.time * frequency) * magnitude;
		gameObject.transform.position += newHeight;
	}
}
