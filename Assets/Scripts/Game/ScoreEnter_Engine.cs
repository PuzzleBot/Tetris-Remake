using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreEnter_Engine : MonoBehaviour {
	GameObject inputField;
	GameObject defeatMenu;
	GameObject defeatScoreSaveScreen;

	// Use this for initialization
	void Awake () {
		inputField = GameObject.Find ("DefeatOverlayCanvas/Model_ScoreMenu/NameInputField");
		defeatMenu = GameObject.Find ("DefeatOverlayCanvas/Model_DefeatMenu");
		defeatScoreSaveScreen = GameObject.Find ("DefeatOverlayCanvas/Model_ScoreMenu");
		defeatMenu.SetActive (false);

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Return)) {
			/*Save the highscore, and show the defeat menu*/
			defeatMenu.SetActive (true);
			defeatScoreSaveScreen.SetActive (false);
		}
	}
}
