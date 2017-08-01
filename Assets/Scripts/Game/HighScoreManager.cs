using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class HighScoreManager : MonoBehaviour {
	private List<HighScoreRecord> scoreList;

	// Use this for initialization
	void Awake () {
		
	}

	public HighScoreRecord getHighScoreAtRanking(int rank){
		return (HighScoreRecord)scoreList [rank];
	}

	public void addHighScore(HighScoreRecord newHighScore){
		scoreList.Add (newHighScore);
		scoreList.Sort ();
		saveToFile (Application.persistentDataPath + "/HighScores.scorec");
	}

	/*Reads a binary file containing the arraylist of highscores into the score list*/
	public void initializeFromFile(string fileName){
		FileStream stream = new FileStream (fileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
		BinaryFormatter formatter = new BinaryFormatter ();

		try{
			scoreList = (List<HighScoreRecord>)formatter.Deserialize (stream);
		} catch(Exception e){
			
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
}
