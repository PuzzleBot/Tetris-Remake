using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionButton : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		GetComponent<Button> ().onClick.AddListener (showOptionScreen);
	}
	
	public void showOptionScreen(){
		SceneManager.LoadScene ("Option Screen", LoadSceneMode.Additive);
		SceneManager.SetActiveScene (SceneManager.GetSceneByName("Option Screen"));
	}
}
