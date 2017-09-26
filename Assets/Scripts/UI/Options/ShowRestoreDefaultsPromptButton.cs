using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowRestoreDefaultsPromptButton : MonoBehaviour {
	private static GameObject restoreDefaultsUI = null;

	// Use this for initialization
	void Start () {
		if (restoreDefaultsUI == null) {
			restoreDefaultsUI = GameObject.Find ("OptionUI/KeyBindMenu/RestoreDefaultsPrompt");
			restoreDefaultsUI.SetActive (false);
		}

		GetComponent<Button> ().onClick.AddListener (showPrompt);
	}

	public void showPrompt(){
		restoreDefaultsUI.SetActive (true);
	}
}
