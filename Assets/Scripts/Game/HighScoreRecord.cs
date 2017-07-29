using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HighScoreRecord{
	private int scoreValue;
	private string name;
	private int numberOfDuplicates;

	public HighScoreRecord(string name, int value){
		this.name = name;
		this.scoreValue = value;
		this.numberOfDuplicates = 1;
	}

	public int getScore(){
		return scoreValue;
	}

	public string getName(){
		return name;
	}

	public int getNumberOfDuplicates(){
		return numberOfDuplicates;
	}
}
