using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadMainMenu()
	{
		SceneManager.LoadScene(0);
	}

	public void LoadStartGame()
	{
		SceneManager.LoadScene(1);
	}

	public void LoadOptions()
	{
		SceneManager.LoadScene(2);
	}

	public void LoadControls()
	{
		SceneManager.LoadScene(3);
	}

	public void LoadGameOver()
	{
		SceneManager.LoadScene(4);
	}

	public void LoadQuit()
	{
		Application.Quit();
	}
	
}
