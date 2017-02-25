using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlockFactory{
	public TetrisBlockFactory(){
		
	}

	public TetrisBlock createNewBlock(int blockType){
		/*Create a random block if the given type is 0*/
		if (blockType == 0) {
			blockType = UnityEngine.Random.Range (1, 8);
		}

		switch (blockType) {
		case 1:
			return(new TetrisLineBlock ());
		case 2:
			return(new TetrisSBlock ());
		case 3:
			return(new TetrisZBlock ());
		case 4:
			return(new TetrisTBlock ());
		case 5:
			return(new TetrisLBlock ());
		case 6:
			return(new TetrisBackLBlock ());
		case 7:
			return(new TetrisSquareBlock ());
		default:
			return(new TetrisLineBlock ());
			
		}
	}
}
