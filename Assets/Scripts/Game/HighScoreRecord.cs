using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HighScoreRecord : IComparable<HighScoreRecord>{
	public const int nameDisplayLimit = 10;
	private int scoreValue;
	private string name;

	public HighScoreRecord(string name, int value){
		this.name = name;
		this.scoreValue = value;
	}

	public int getScore(){
		return scoreValue;
	}

	public string getName(){
		return name;
	}

	/*Compare method for sorting, uses the score as the value to determine which score record is greater
	 *https://msdn.microsoft.com/en-us/library/w56d4y5z(v=vs.110).aspx?cs-save-lang=1&cs-lang=csharp#code-snippet-2*/
	public int CompareTo(HighScoreRecord compareRecord){
		if (compareRecord == null) {
			return 1;
		} else {
			return this.scoreValue.CompareTo (compareRecord.getScore ());
		}
	}

	public override string ToString(){
		if (name.Length <= nameDisplayLimit) {
			return name + " - " + scoreValue.ToString ();
		} else {
			return name.Substring(0, nameDisplayLimit) + "... - " + scoreValue.ToString ();
		}
	}

	public HighScoreRecord Clone(){
		return new HighScoreRecord (this.name, this.scoreValue);
	}
}
