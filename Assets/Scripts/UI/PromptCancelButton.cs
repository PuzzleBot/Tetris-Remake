using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromptCancelButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Button> ().onClick.AddListener (hidePrompt);
	}
	
	public void hidePrompt(){
		transform.parent.gameObject.SetActive (false);
	}
}
