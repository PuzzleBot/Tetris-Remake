using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBindButton : MonoBehaviour {
	private static GameObject keyBindUI = null;
	private static KeyModifyTracker modifyTracker = null;
	private string actionToBind;

	// Use this for initialization
	void Start () {
		if (keyBindUI == null) {
			keyBindUI = GameObject.Find ("OptionUI/KeyBindMenu/BindKeyPrompt");
			keyBindUI.SetActive (false);
		}

		if (modifyTracker == null) {
			modifyTracker = GameObject.Find ("OptionUI/KeyBindMenu").GetComponent<KeyModifyTracker> ();
		}

		actionToBind = transform.parent.gameObject.name;
		updateKeyText ();

		GetComponent<Button> ().onClick.AddListener (showKeyBindUI);

		//Debug.Log (actionToBind);
	}
	
	public void showKeyBindUI(){
		modifyTracker.actionToModify = actionToBind;
		keyBindUI.GetComponent<KeyBind_KeyListener> ().activate ();
		keyBindUI.SetActive (true);
	}

	public void updateKeyText(){
		transform.Find ("Text").GetComponent<Text>().text = KeyBindings.getInstance().getBoundKeyString(actionToBind);
	}
}
