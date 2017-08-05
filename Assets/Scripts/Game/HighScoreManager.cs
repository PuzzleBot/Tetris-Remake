using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighScoreManager : MonoBehaviour {
	private List<HighScoreRecord> scoreList;
	private static string scoreFileName = "/HighScores.scorec";

	// Use this for initialization
	void Start () {
		initializeFromFile (Application.persistentDataPath + scoreFileName);
		if (GameObject.Find ("HighScoreUI") != null) {
			populateHighScoreScreen ();
		}

		Debug.Log (this.ToString());
	}

	public HighScoreRecord getHighScoreAtRanking(int rank){
		return (HighScoreRecord)scoreList [rank];
	}

	public void addHighScore(HighScoreRecord newHighScore){
		scoreList.Add (newHighScore);
		scoreList.Sort ();

		/*Only the 10 highest scores are needed*/
		if (scoreList.Count > 10) {
			scoreList.RemoveAt (10);
		}

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

	public void populateHighScoreScreen(){
		int i;
		Debug.Log ("Populating Score Screen...");
		/*Display the top 10 highscores on the highscore UI*/
		if (scoreList.Count > 10) {
			for (i = 0; i < 10; i++) {
				updateHighScoreUIBox (10 - i, scoreList[i]);
			}
		} else {
			/*If there are less than 10 highscores, display them all and
			 *fill the rest of the text boxes with blanks*/
			for (i = 0; i < scoreList.Count; i++) {
				updateHighScoreUIBox (scoreList.Count - i, scoreList[i]);
			}
			for (i = scoreList.Count; i < 10; i++) {
				updateHighScoreUIBox (i + 1, null);
			}
		}
	}

	private void updateHighScoreUIBox(int ranking, HighScoreRecord record){
		/*Update the UI text box that is supposed to hold the corresponding rank of highscore*/
		if ((ranking > 10) || (ranking < 1)) {
			throw new IndexOutOfRangeException ("Only high scores ranked 1 - 10 are allowed.");
		} else {
			string scoreObjectName = "HighScore" + ranking.ToString ();

			if(record == null){
				GameObject.Find ("HighScoreUI/" + scoreObjectName).GetComponent<Text>().text = ranking.ToString() + ". - ";
			}
			else{
				GameObject.Find ("HighScoreUI/" + scoreObjectName).GetComponent<Text>().text = ranking.ToString() + ". " + record.ToString();
			}
		}
	}
}
