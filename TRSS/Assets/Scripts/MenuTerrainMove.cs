using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTerrainMove : MonoBehaviour
{
	
	public float speed;
	private Terrain terrain; 
	// A splat prototype is just a texture on a terrain.
	private SplatPrototype[] splatPrototypes;


	// Use this for initialization
	void Start ()
	{
		terrain = gameObject.GetComponent<Terrain>();
		splatPrototypes = terrain.terrainData.splatPrototypes;
	}
	
	// Update is called once per frame
	void Update ()
	{
		splatPrototypes[0].tileOffset += new Vector2(-speed, 0);
		terrain.terrainData.splatPrototypes = splatPrototypes;
	}
}
