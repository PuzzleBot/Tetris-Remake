using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score{
	private int scoreValue;
	private string name;

	public Score(string name, int value){
		this.name = name;
		this.scoreValue = value;
	}

	public int getScore(){
		return scoreValue;
	}

	public string getName(){
		return name;
	}
}
