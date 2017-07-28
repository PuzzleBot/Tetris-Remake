using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScore{
	private int scoreValue;
	private int tetrisStreak;
	private int linesCleared;
	private int level;

	public GameScore(){
		scoreValue = 0;
		tetrisStreak = 0;
		linesCleared = 0;
		level = 0;
	}

	public void addLineClearScore(int numberOfLinesCleared){
		/*Original Nintendo Scoring System: http://tetris.wikia.com/wiki/Scoring*/
		switch (numberOfLinesCleared) {
		case 1:
			scoreValue = scoreValue + (40 * (level + 1));
			tetrisStreak = 0;
			break;
		case 2:
			scoreValue = scoreValue + (100 * (level + 1));
			tetrisStreak = 0;
			break;
		case 3:
			scoreValue = scoreValue + (300 * (level + 1));
			tetrisStreak = 0;
			break;
		case 4:
			/*Score increase multiplied by 1.5 for each tetris before this one*/
			scoreValue = scoreValue + ((1200 * (level + 1)) * ((int)(tetrisStreak * 1.5) + 1));
			tetrisStreak++;
			break;
		default:
			break;
		}

		linesCleared = linesCleared + numberOfLinesCleared;
		level = linesCleared / 10;
	}

	public int getScoreValue(){
		return scoreValue;
	}

	public int getStreak(){
		return tetrisStreak;
	}

	public int getLevel(){
		return level;
	}
}
