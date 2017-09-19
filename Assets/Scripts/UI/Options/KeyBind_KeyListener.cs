using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBind_KeyListener : MonoBehaviour {
	public static KeyBindings keyBindings = null;
	public bool active;

	// Use this for initialization
	void Start () {
		if(keyBindings == null){
			keyBindings = KeyBindings.getInstance ();
		}

		active = true;
	}
	
	// Update is called once per frame
	void Update () {
		if ((active == true) && (Input.anyKeyDown)) {
			active = false;
			KeyCode pressedKey = Event.current.keyCode;
	
			/*Try to bind the key. If it can't be bound, display an error.*/
			try{
				string actionToModify = GameObject.Find("OptionUI/KeyBindMenu").GetComponent<KeyModifyTracker>().actionToModify;
				keyBindings.bindKey(actionToModify, pressedKey);

				GameObject.Find("OptionUI/KeyBindMenu/" + actionToModify + "/KeybindButton").GetComponent<KeyBindButton>().updateKeyText();
				this.transform.gameObject.SetActive(false);
			}
			catch(InvalidKeyException e){
				Debug.Log (e.ToString ());
			}
			finally{
				active = true;
			}
		}
	}

	public void activate(){
		active = true;
	}
}
