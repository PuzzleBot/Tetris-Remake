using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		/*The button that the script is attached to now activates the title screen loading function on click*/
		GetComponent<Button> ().onClick.AddListener (showHighScore);
	}

	public void showHighScore(){

	}
}
