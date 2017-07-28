using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour {
	public static HighScoreManager manager = null;

	// Use this for initialization
	void Awake () {
		if (manager == null) {
			DontDestroyOnLoad (this);
			manager = this;
		} else {
			GameObject.Destroy (this);
		}
	}
}
