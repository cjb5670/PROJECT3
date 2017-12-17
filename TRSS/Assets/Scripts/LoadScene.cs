using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

	public Animator mainMenu;
	public Animator controlsMenu;
	public Animator creditsMenu;

	public List<GameObject> Animated;


	// Use this for initialization
	void Start ()
	{
		mainMenu = Animated[0].GetComponent<Animator>();
		controlsMenu = Animated[1].GetComponent<Animator>();
		creditsMenu = Animated[2].GetComponent<Animator>();

		// sets animations to default state
		foreach (GameObject a in Animated)
		{
			Animator anim = a.GetComponent<Animator>();
			anim.SetFloat("Condition", 0.0f);
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadMainMenuFromCredits()
	{
		Debug.Log("Enter");
		mainMenu.SetFloat("Condition", 2);
		creditsMenu.SetFloat("Condition", 2);
		Debug.Log("Exit");
	}

	public void LoadMainMenuFromControls()
	{
		Debug.Log("Enter");
		mainMenu.SetFloat("Condition", 2);
		controlsMenu.SetFloat("Condition", 2);
		Debug.Log("Exit");
	}

	public void LoadStartGame()
	{
		SceneManager.LoadScene(1);
	}

	public void LoadCredits()
	{
		Debug.Log("Enter");
		mainMenu.SetFloat("Condition", 1);
		creditsMenu.SetFloat("Condition", 1);
		Debug.Log("Exit");
	}

	public void LoadControls()
	{
		Debug.Log("Enter");
		mainMenu.SetFloat("Condition", 1);
		controlsMenu.SetFloat("Condition", 1);
		Debug.Log("Exit");
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
