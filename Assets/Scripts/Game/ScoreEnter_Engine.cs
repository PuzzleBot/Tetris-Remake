using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreEnter_Engine : MonoBehaviour {
	private GameObject inputFieldText;
	private GameObject defeatMenu;
	private GameObject defeatScoreSaveScreen;
	private HighScoreManager scoreManager;
	private int finalScore;

	// Use this for initialization
	void Awake () {
		inputFieldText = GameObject.Find ("OverlayUI/DefeatOverlayCanvas/Model_ScoreMenu/NameInputField/Text");
		defeatMenu = GameObject.Find ("OverlayUI/DefeatOverlayCanvas/Model_DefeatMenu");
		defeatScoreSaveScreen = GameObject.Find ("OverlayUI/DefeatOverlayCanvas/Model_ScoreMenu");
		scoreManager = GameObject.Find ("OverlayUI/DefeatOverlayCanvas/High Score Holder").GetComponent<HighScoreManager>();

		defeatMenu.SetActive (false);
		defeatScoreSaveScreen.SetActive (false);
		finalScore = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Return)) {
			/*Save the highscore, and show the defeat menu*/
			scoreManager.addHighScore (new HighScoreRecord(inputFieldText.GetComponent<Text>().text, finalScore));
			defeatMenu.SetActive (true);
			defeatScoreSaveScreen.SetActive (false);
		}
	}

	public void setFinalScore(int score){
		finalScore = score;
		if (score == 0) {
			defeatMenu.SetActive (true);
		} else {
			defeatScoreSaveScreen.SetActive (true);
		}
	}
}
