using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		/*The button that the script is attached to now activates the start function on click*/
		GetComponent<Button> ().onClick.AddListener (startGame);
	}
	
	public void startGame(){
		/*Closes the main menu and loads the game*/
		SceneManager.LoadScene ("Main Game", LoadSceneMode.Single);
	}
}
