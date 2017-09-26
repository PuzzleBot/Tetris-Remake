using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBindings{
	/*Singleton Pattern*/
	private static KeyBindings instance = null;

	/*Note: Only alphanumeric keys, arrow keys, Shift, and Space are allowed.*/
	private string accelerateDownKey;
	private string forceDownKey;
	private string moveLeftKey;
	private string moveRightKey;
	private string rotateClockwiseKey;
	private string rotateCounterClockwiseKey;
	private string savePieceKey;
	private string pauseKey;

	//public enum KeyAction {AccelerateDown, ForceDown, MoveLeft, MoveRight, RotateClockwise, RotateCounterClockwise, SavePiece, Pause};

	//private string[] keyBindArray;

	/*Private intitialization - only one instance of this is allowed.*/
	private KeyBindings() {
		/*Keybindings are stored in an array for easy iteration*/
		//keyBindArray = new string[8];

		/*Set the keybinds to their defaults if they are unset, otherwise initialize
		the keybinds to what they previously were*/
		if (!PlayerPrefs.HasKey ("KeyBind_AccelerateDown")) {
			bindKey ("AccelerateDown", KeyCode.S);
		} else {
			accelerateDownKey = PlayerPrefs.GetString ("KeyBind_AccelerateDown");
		}

		if (!PlayerPrefs.HasKey ("KeyBind_ForceDown")) {
			bindKey ("ForceDown", KeyCode.W);
		} else {
			forceDownKey = PlayerPrefs.GetString ("KeyBind_ForceDown");
		}

		if (!PlayerPrefs.HasKey ("KeyBind_MoveLeft")) {
			bindKey ("MoveLeft", KeyCode.A);
		} else {
			moveLeftKey = PlayerPrefs.GetString ("KeyBind_MoveLeft");
		}

		if (!PlayerPrefs.HasKey ("KeyBind_MoveRight")) {
			bindKey ("MoveRight", KeyCode.D);
		} else {
			moveRightKey = PlayerPrefs.GetString ("KeyBind_MoveRight");
		}

		if (!PlayerPrefs.HasKey ("KeyBind_RotateClockwise")) {
			bindKey ("RotateClockwise", KeyCode.E);
		} else {
			rotateClockwiseKey = PlayerPrefs.GetString ("KeyBind_RotateClockwise");
		}

		if (!PlayerPrefs.HasKey ("KeyBind_RotateCounterClockwise")) {
			bindKey ("RotateCounterClockwise", KeyCode.Q);
		} else {
			rotateCounterClockwiseKey = PlayerPrefs.GetString ("KeyBind_RotateCounterClockwise");
		}

		if (!PlayerPrefs.HasKey ("KeyBind_SavePiece")) {
			bindKey ("SavePiece", KeyCode.Space);
		} else {
			savePieceKey = PlayerPrefs.GetString ("KeyBind_SavePiece");
		}

		if (!PlayerPrefs.HasKey ("KeyBind_Pause")) {
			bindKey ("Pause", KeyCode.P);
		} else {
			pauseKey = PlayerPrefs.GetString ("KeyBind_Pause");
		}

		PlayerPrefs.Save ();
		Debug.Log (ToString ());
	}

	/*Singleton - instance access method*/
	public static KeyBindings getInstance(){
		if (instance == null) {
			instance = new KeyBindings ();
		}
		return instance;
	}

	public void bindKey(string toModify, KeyCode key){
		string keyString = key.ToString ();
		if ((key.ToString ().Length > 1) && 
			(!key.ToString().Equals("Space")) &&
			(!key.ToString().Equals("LeftArrow")) &&
			(!key.ToString().Equals("RightArrow")) &&
			(!key.ToString().Equals("UpArrow")) &&
			(!key.ToString().Equals("DownArrow")) && 
			(!key.ToString().Equals("LeftShift")) &&
			(!key.ToString().Equals("RightShift"))) {

			throw new InvalidKeyException ("\"" + key.ToString() + "\" cannot be binded. Please choose another key.");
		}

		if (keyString.Length <= 1) {
			keyString.ToLower ();
		}

		switch (toModify) {
		case "AccelerateDown":
			accelerateDownKey = keyString;
			PlayerPrefs.SetString ("KeyBind_AccelerateDown", accelerateDownKey);
			break;
		case "ForceDown":
			forceDownKey = keyString;
			PlayerPrefs.SetString ("KeyBind_ForceDown", forceDownKey);
			break;
		case "MoveLeft":
			moveLeftKey = keyString;
			PlayerPrefs.SetString ("KeyBind_MoveLeft", moveLeftKey);
			break;
		case "MoveRight":
			moveRightKey = keyString;
			PlayerPrefs.SetString ("KeyBind_MoveRight", moveRightKey);
			break;
		case "RotateClockwise":
			rotateClockwiseKey = keyString;
			PlayerPrefs.SetString ("KeyBind_RotateClockwise", rotateClockwiseKey);
			break;
		case "RotateCounterClockwise":
			rotateCounterClockwiseKey = keyString;
			PlayerPrefs.SetString ("KeyBind_RotateCounterClockwise", rotateCounterClockwiseKey);
			break;
		case "SavePiece":
			savePieceKey = keyString;
			PlayerPrefs.SetString ("KeyBind_SavePiece", savePieceKey);
			break;
		case "Pause":
			pauseKey = keyString;
			PlayerPrefs.SetString ("KeyBind_Pause", pauseKey);
			break;
		default:
			throw new InvalidKeyException ("Error: Invalid key action - " + toModify);
		}

		PlayerPrefs.Save ();
	}

	/*Gets the key bound to the action*/
	public KeyCode getBoundKey(string keyAction){
		KeyCode key = KeyCode.Space;
		string storedKeyString;

		try{
			storedKeyString = getBoundKeyString (keyAction);
		} catch (InvalidKeyException e){
			throw e;
		}
			
		if (storedKeyString.Length == 1) {
			key = (KeyCode) System.Enum.Parse(typeof(KeyCode), storedKeyString);
		} else {
			switch (storedKeyString) {
			case "Space":
				key = KeyCode.Space;
				break;
			case "LeftShift":
				key = KeyCode.LeftShift;
				break;
			case "RightShift":
				key = KeyCode.RightShift;
				break;
			case "LeftArrow":
				key = KeyCode.LeftArrow;
				break;
			case "RightArrow":
				key = KeyCode.RightArrow;
				break;
			case "DownArrow":
				key = KeyCode.DownArrow;
				break;
			case "UpArrow":
				key = KeyCode.UpArrow;
				break;
			default:
				Debug.Log ("Invalid key: " + storedKeyString);
				key = KeyCode.Space;
				break;
			}
		}

		return key;
	}

	/*Gets the key bound to the action, in string form*/
	public string getBoundKeyString(string keyAction){
		string storedKeyString;

		switch (keyAction) {
		case "AccelerateDown":
			storedKeyString = accelerateDownKey;
			break;
		case "ForceDown":
			storedKeyString = forceDownKey;
			break;
		case "MoveLeft":
			storedKeyString = moveLeftKey;
			break;
		case "MoveRight":
			storedKeyString = moveRightKey;
			break;
		case "RotateClockwise":
			storedKeyString = rotateClockwiseKey;
			break;
		case "RotateCounterClockwise":
			storedKeyString = rotateCounterClockwiseKey;
			break;
		case "SavePiece":
			storedKeyString = savePieceKey;
			break;
		case "Pause":
			storedKeyString = pauseKey;
			break;
		default:
			throw new InvalidKeyException ("Error: Invalid key action: " + keyAction);
		}

		return storedKeyString;
	}

	public override string ToString(){
		return "AccelDown: " + accelerateDownKey + "\n" +
			"ForceDown: " + forceDownKey + "\n" +
			"MoveLeft: " + moveLeftKey + "\n" +
			"MoveRight: " + moveRightKey + "\n" +
			"RotateClockwise: " + rotateClockwiseKey + "\n" +
			"RotateCCW: " + rotateCounterClockwiseKey + "\n" +
			"SavePiece: " + savePieceKey + "\n" +
			"Pause: " + pauseKey + "\n";
	}

	public void resetToDefaults(){
		bindKey ("AccelerateDown", KeyCode.S);
		bindKey ("ForceDown", KeyCode.W);
		bindKey ("MoveLeft", KeyCode.A);
		bindKey ("MoveRight", KeyCode.D);
		bindKey ("RotateClockwise", KeyCode.E);
		bindKey ("RotateCounterClockwise", KeyCode.Q);
		bindKey ("SavePiece", KeyCode.Space);
		bindKey ("Pause", KeyCode.P);
	}
}
