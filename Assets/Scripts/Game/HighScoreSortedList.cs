using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*SortedList decorator for handling duplicates*/
public class HighScoreSortedList : SortedList {

	public HighScoreSortedList() : base(){
		
	}

	public void Add(int key, HighScoreRecord value){
		base.Add (key, value);
	}
}
