using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestoreDefaultKeybindsButton : MonoBehaviour {
	private static KeyBindings keybindings = null;

	/*References to buttons on the keybind menu to update their text when keybindings are reset*/
	private static KeyBindButton moveLeftButton = null;
	private static KeyBindButton moveRightButton = null;
	private static KeyBindButton rotateClockwiseButton = null;
	private static KeyBindButton rotateCounterClockwiseButton = null;
	private static KeyBindButton accelerateDownButton = null;
	private static KeyBindButton forceDownButton = null;
	private static KeyBindButton savePieceButton = null;
	private static KeyBindButton pauseButton = null;

	// Use this for initialization
	void Start () {
		if (keybindings == null) {
			keybindings = KeyBindings.getInstance ();
		}


		if (moveLeftButton == null) {
			moveLeftButton = GameObject.Find ("OptionUI/KeyBindMenu/MoveLeft/KeybindButton").GetComponent<KeyBindButton> ();
		}

		if (moveRightButton == null) {
			moveRightButton = GameObject.Find ("OptionUI/KeyBindMenu/MoveRight/KeybindButton").GetComponent<KeyBindButton> ();
		}

		if (rotateClockwiseButton == null) {
			rotateClockwiseButton = GameObject.Find ("OptionUI/KeyBindMenu/RotateClockwise/KeybindButton").GetComponent<KeyBindButton> ();
		}

		if (rotateCounterClockwiseButton == null) {
			rotateCounterClockwiseButton = GameObject.Find ("OptionUI/KeyBindMenu/RotateCounterClockwise/KeybindButton").GetComponent<KeyBindButton> ();
		}

		if (accelerateDownButton == null) {
			accelerateDownButton = GameObject.Find ("OptionUI/KeyBindMenu/AccelerateDown/KeybindButton").GetComponent<KeyBindButton> ();
		}

		if (forceDownButton == null) {
			forceDownButton = GameObject.Find ("OptionUI/KeyBindMenu/ForceDown/KeybindButton").GetComponent<KeyBindButton> ();
		}

		if (savePieceButton == null) {
			savePieceButton = GameObject.Find ("OptionUI/KeyBindMenu/SavePiece/KeybindButton").GetComponent<KeyBindButton> ();
		}

		if (pauseButton == null) {
			pauseButton = GameObject.Find ("OptionUI/KeyBindMenu/Pause/KeybindButton").GetComponent<KeyBindButton> ();
		}

		GetComponent<Button> ().onClick.AddListener (restoreDefaultKeys);
	}
	
	public void restoreDefaultKeys(){
		keybindings.resetToDefaults ();
		transform.parent.gameObject.SetActive (false);

		moveLeftButton.updateKeyText ();
		moveRightButton.updateKeyText ();
		rotateClockwiseButton.updateKeyText ();
		rotateCounterClockwiseButton.updateKeyText ();
		accelerateDownButton.updateKeyText ();
		forceDownButton.updateKeyText ();
		savePieceButton.updateKeyText ();
		pauseButton.updateKeyText ();
	}
}
