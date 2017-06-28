using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		/*Use the quit method as the listener method*/
		GetComponent<Button> ().onClick.AddListener (Application.Quit);
	}
}
