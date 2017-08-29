using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBindings : MonoBehaviour {
	/*Note: Only alphanumeric keys, arrow keys and SPACE are allowed.*/
	private string accelerateDownKey;
	private string forceDownKey;
	private string moveLeftKey;
	private string moveRightKey;
	private string rotateClockwiseKey;
	private string rotateCounterClockwiseKey;
	private string savePieceKey;
	private string pauseKey;

	// Use this for initialization
	void Start () {
		/*Set the keybinds to their defaults if they are unset, otherwise initialize
		  the keybinds to what they previously were*/
		if (!PlayerPrefs.HasKey ("KeyBind_AccelerateDown")) {
			accelerateDownKey = "s";
			PlayerPrefs.SetString ("KeyBind_AccelerateDown", accelerateDownKey);
		} else {
			accelerateDownKey = PlayerPrefs.GetString("KeyBind_AccelerateDown");
		}

		if (!PlayerPrefs.HasKey ("KeyBind_ForceDown")) {
			forceDownKey = "w";
			PlayerPrefs.SetString ("KeyBind_ForceDown", forceDownKey);
		} else {
			forceDownKey = PlayerPrefs.GetString("KeyBind_ForceDown");
		}

		if (!PlayerPrefs.HasKey ("KeyBind_MoveLeft")) {
			moveLeftKey = "a";
			PlayerPrefs.SetString ("KeyBind_MoveLeft", moveLeftKey);
		} else {
			moveLeftKey = PlayerPrefs.GetString("KeyBind_MoveLeft");
		}

		if (!PlayerPrefs.HasKey ("KeyBind_MoveRight")) {
			moveRightKey = "d";
			PlayerPrefs.SetString ("KeyBind_MoveRight", moveRightKey);
		} else {
			moveRightKey = PlayerPrefs.GetString("KeyBind_MoveRight");
		}

		if (!PlayerPrefs.HasKey ("KeyBind_RotateClockwise")) {
			rotateClockwiseKey = "e";
			PlayerPrefs.SetString ("KeyBind_RotateClockwise", rotateClockwiseKey);
		} else {
			rotateClockwiseKey = PlayerPrefs.GetString("KeyBind_RotateClockwise");
		}

		if (!PlayerPrefs.HasKey ("KeyBind_RotateCounterClockwise")) {
			rotateCounterClockwiseKey = "q";
			PlayerPrefs.SetString ("KeyBind_RotateCounterClockwise", rotateCounterClockwiseKey);
		} else {
			rotateCounterClockwiseKey = PlayerPrefs.GetString("KeyBind_RotateCounterClockwise");
		}

		if (!PlayerPrefs.HasKey ("KeyBind_SavePiece")) {
			savePieceKey = "SPACE";
			PlayerPrefs.SetString ("KeyBind_SavePiece", savePieceKey);
		} else {
			savePieceKey = PlayerPrefs.GetString("KeyBind_SavePiece");
		}

		if (!PlayerPrefs.HasKey ("KeyBind_Pause")) {
			pauseKey = "p";
			PlayerPrefs.SetString ("KeyBind_Pause", pauseKey);
		} else {
			pauseKey = PlayerPrefs.GetString("KeyBind_Pause");
		}

		PlayerPrefs.Save ();
	}

	public void bindKey(string toModify, Object key){
		
	}
}
