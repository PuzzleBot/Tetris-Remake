using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class HighScoreManager : MonoBehaviour {
	private List<HighScoreRecord> scoreList;
	private static string scoreFileName = "/HighScores.scorec";

	// Use this for initialization
	void Awake () {
		initializeFromFile (Application.persistentDataPath + scoreFileName);
		Debug.Log (this.ToString());
	}

	public HighScoreRecord getHighScoreAtRanking(int rank){
		return (HighScoreRecord)scoreList [rank];
	}

	public void addHighScore(HighScoreRecord newHighScore){
		scoreList.Add (newHighScore);
		scoreList.Sort ();
		saveToFile (Application.persistentDataPath + scoreFileName);
	}

	/*Reads a binary file containing the arraylist of highscores into the score list*/
	public void initializeFromFile(string fileName){
		FileStream stream = new FileStream (fileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
		BinaryFormatter formatter = new BinaryFormatter ();

		try{
			scoreList = (List<HighScoreRecord>)formatter.Deserialize (stream);
		} catch(Exception e){
			scoreList = new List<HighScoreRecord> ();
		}


		stream.Close ();
	}

	/*Saves the score list as a binary file*/
	public void saveToFile(string fileName){
		FileStream stream = new FileStream (fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
		BinaryFormatter formatter = new BinaryFormatter ();
		formatter.Serialize (stream, scoreList);
		stream.Close ();
	}

	public override string ToString(){
		string scoreString = "";

		foreach (HighScoreRecord r in scoreList) {
			scoreString = scoreString + r.ToString();
			scoreString = scoreString + "\n";
		}

		return scoreString;
	}
}
