using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class HighScoreManager : MonoBehaviour {
	private SortedList<int, HighScoreRecord> scoreList;

	// Use this for initialization
	void Awake () {

	}

	public HighScoreRecord getHighScoreAtRanking(int rank){
		return (HighScoreRecord)scoreList [rank];
	}

	public void addHighScore(HighScoreRecord newHighScore){
		scoreList.Add (newHighScore.getScore(), newHighScore);
		saveToFile (Application.persistentDataPath + "/HighScores.scorec");
	}

	/*Reads a binary file containing the arraylist of highscores into the score list*/
	public void initializeFromFile(string fileName){
		FileStream stream = new FileStream (fileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
		BinaryFormatter formatter = new BinaryFormatter ();
		scoreList = (SortedList<int, HighScoreRecord>)formatter.Deserialize (stream);
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
