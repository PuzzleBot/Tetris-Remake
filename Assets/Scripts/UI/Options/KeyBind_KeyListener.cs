using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBind_KeyListener : MonoBehaviour {
	public static KeyBindings keyBindings = null;
	private GameObject allowedKeyText;
	public bool active;

	// Use this for initialization
	void Start () {
		if(keyBindings == null){
			keyBindings = KeyBindings.getInstance ();
		}

		allowedKeyText = transform.Find ("AllowedKeyText").gameObject;
		active = true;
	}

	/*When the user presses a key, attempt to bind it*/
	void OnGUI(){
		Event currentEvent = Event.current;
		if ((active == true) && (currentEvent != null) && (currentEvent.isKey == true)) {
			active = false;
			KeyCode pressedKey = currentEvent.keyCode;

			if (pressedKey == KeyCode.Escape) {
				hideBindingPrompt ();
			} else {
				/*Try to bind the key. If it can't be bound, display an error.*/
				try {
					string actionToModify = GameObject.Find ("OptionUI/KeyBindMenu").GetComponent<KeyModifyTracker> ().actionToModify;
					keyBindings.bindKey (actionToModify, pressedKey);

					GameObject.Find ("OptionUI/KeyBindMenu/" + actionToModify + "/KeybindButton").GetComponent<KeyBindButton> ().updateKeyText ();
					hideBindingPrompt ();
				} catch (InvalidKeyException e) {
					Debug.Log (e.ToString ());
					allowedKeyText.GetComponent<BlinkableText> ().blink ();
				} finally {
					active = true;
				}
			}
		}
	}

	public void hideBindingPrompt(){
		this.transform.gameObject.SetActive(false);
	}

	public void activate(){
		active = true;
	}
}
