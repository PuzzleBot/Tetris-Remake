using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreEnter_Engine : MonoBehaviour {
	private GameObject inputField;
	private GameObject defeatMenu;
	private GameObject defeatScoreSaveScreen;
	private int finalScore;

	// Use this for initialization
	void Awake () {
		inputField = GameObject.Find ("DefeatOverlayCanvas/Model_ScoreMenu/NameInputField");
		defeatMenu = GameObject.Find ("DefeatOverlayCanvas/Model_DefeatMenu");
		defeatScoreSaveScreen = GameObject.Find ("DefeatOverlayCanvas/Model_ScoreMenu");
		defeatMenu.SetActive (false);
		finalScore = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Return)) {
			/*Save the highscore, and show the defeat menu*/
			defeatMenu.SetActive (true);
			defeatScoreSaveScreen.SetActive (false);
		}
	}

	public void setFinalScore(int score){
		finalScore = score;
	}
}
