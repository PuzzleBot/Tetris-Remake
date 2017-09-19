using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionBackButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		/*Whatever button this is attached to will now close the holding scene when clicked*/
		GetComponent<Button> ().onClick.AddListener (closeScene);
	}

	public void closeScene (){
		//SceneManager.SetActiveScene (SceneManager.GetSceneByName("Main Game"));
		SceneManager.UnloadSceneAsync ("Option Screen");
	}
}
