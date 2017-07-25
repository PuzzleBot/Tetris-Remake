using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		/*The button that the script is attached to now activates the title screen loading function on click*/
		GetComponent<Button> ().onClick.AddListener (titleScreen);
	}
	
	public void titleScreen(){
		SceneManager.LoadScene ("Title Screen", LoadSceneMode.Single);
	}
}
