using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreEnter_Engine : MonoBehaviour {
	GameObject inputField;
	GameObject defeatMenu;

	// Use this for initialization
	void Awake () {
		inputField = GameObject.Find ("DefeatOverlayCanvas/Model_ScoreMenu/NameInputField");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Return)) {
			/*Save the highscore, and show the defeat menu*/
		}
	}
}
