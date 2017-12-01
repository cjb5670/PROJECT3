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

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

	public void LoadMainMenuFromCredits()
	{
		mainMenu.SetFloat("Condition", 2);
		creditsMenu.SetFloat("Condition", 2);
	}

	public void LoadMainMenuFromControls()
	{
		mainMenu.SetFloat("Condition", 2);
		controlsMenu.SetFloat("Condition", 2);
	}

	public void LoadStartGame()
	{
		SceneManager.LoadScene(5);
	}

	public void LoadCredits()
	{
		mainMenu.SetFloat("Condition", 1);
		creditsMenu.SetFloat("Condition", 1);
	}

	public void LoadControls()
	{
		mainMenu.SetFloat("Condition", 1);
		controlsMenu.SetFloat("Condition", 1);
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
