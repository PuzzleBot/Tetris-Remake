using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkableText : MonoBehaviour {
	private int timerCount;
	private bool isOriginalColor;

	/*How many frames should pass between color changes*/
	public int blinkTiming;

	/*How many times the component should change to the other color*/
	public int blinkCount;

	public Color originalColor;
	public Color blinkColor;

	// Use this for initialization
	void Awake () {
		timerCount = 0;
		isOriginalColor = true;

		blinkTiming = 20;
		blinkCount = 3;

		originalColor = new Color((float)1.0, (float)1.0, (float)1.0);
		blinkColor = new Color ((float)1.0, (float)0.5, (float)0.5);
	}
	
	// Update is called once per frame
	void Update () {
		if (timerCount > 0) {
			timerCount--;
			if (timerCount == 0) {
				/*At the end of the blink, reset the component's color*/
				transform.GetComponent<Text> ().color = originalColor;
			} else {
				/*Change colors whenever <blinktiming> frames pass*/
				if ((timerCount % blinkTiming) == 0) {
					if (isOriginalColor) {
						transform.GetComponent<Text> ().color = blinkColor;
						isOriginalColor = false;
					} else {
						transform.GetComponent<Text> ().color = originalColor;
						isOriginalColor = true;
					}
				}
			}
		}
	}

	public void blink(){
		timerCount = blinkTiming * blinkCount * 2;
	}
}
