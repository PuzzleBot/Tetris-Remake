using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighScoreButton : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		/*The button that the script is attached to now activates the title screen loading function on click*/
		GetComponent<Button> ().onClick.AddListener (showHighScore);
	}

	public void showHighScore(){
		SceneManager.LoadScene ("High Score Screen", LoadSceneMode.Additive);
		SceneManager.SetActiveScene (SceneManager.GetSceneByName("High Score Screen"));
	}
}
