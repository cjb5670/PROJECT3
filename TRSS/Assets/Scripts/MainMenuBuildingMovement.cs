using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBuildingMovement : MonoBehaviour
{
	// Becomes a range of numbers relative to the parent that objects will spawn.
	// X is min and Y is max
	public Vector2 spawnZone;
	// Area releative to the parent that building will respawn
	public int respawnLine;
	public float speed;


	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		gameObject.transform.position += new Vector3(speed, 0, 0);	

		// When over respawn line, resets to random position within standards defined by spawnZone
		if (gameObject.transform.position.x > respawnLine)
		{
			gameObject.transform.position = new Vector3(Random.Range(spawnZone.x, spawnZone.y),
				gameObject.transform.position.y,
				gameObject.transform.position.z);
		}

	}
}
