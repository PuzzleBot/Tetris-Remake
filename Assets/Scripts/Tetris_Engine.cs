using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetris_Engine : MonoBehaviour {
	private GameGrid grid;

	private TetrisBlock currentPiece;
	private TetrisBlock nextPiece;

	private int gravityCounter;
	private int gravityUpdateCount;

	private int keyCounter;
	private int keyCooldown;

	private bool gameIsPaused;

	// Use this for initialization
	public void Start () {
		grid = new GameGrid ();

		/*Piece buffer initialization*/
		currentPiece = new TetrisBlock (0);
		nextPiece = new TetrisBlock (0);

		moveNextPieceToCurrent ();
		generateNextPiece ();

		gravityCounter = 0;
		gravityUpdateCount = 30;

		keyCounter = 0;
		keyCooldown = 10;

		gameIsPaused = false;

	}
	
	// Update is called once per frame
	public void Update () {
		/*Game paused*/
		if(Input.GetKey("p")){
			togglePause ();
		}

		if (gameIsPaused == true) {
			return;
		}

		gravityCounter++;

		/*Key check*/
		if (keyCounter < keyCooldown) {
			keyCounter++;
		}
		else {
			if (Input.GetKey ("a")) {
				/*Collision check for left movement*/
				if(!grid.collision(TetrisBlock.MoveType.LEFT, currentPiece)){
					currentPiece.shiftLeft ();
				}
				keyCounter = 0;
			} else if (Input.GetKey ("d")) {
				if(!grid.collision(TetrisBlock.MoveType.RIGHT, currentPiece)){
					currentPiece.shiftRight ();
				}
				keyCounter = 0;
			}
		}

		/*Down key accelerates gravity*/
		if(Input.GetKeyDown("s")){
			gravityUpdateCount = 5;
		}
		else if (Input.GetKeyUp ("s")) {
			gravityUpdateCount = 30;
		}

		/*Gravity tick per half second*/
		if (gravityCounter >= gravityUpdateCount) {
			if (!grid.collision (TetrisBlock.MoveType.DOWN, currentPiece)) {
				/*Move down if there is no collision*/
				currentPiece.gravity ();
			} else {
				moveNextPieceToCurrent ();
				generateNextPiece ();
			}
			gravityCounter = 0;
		}
	}

	/*Pick a new piece at random*/
	public void generateNextPiece(){
		nextPiece.changeToRandom ();
	}

	public void moveNextPieceToCurrent(){
		currentPiece.changeType (nextPiece.getBlockType());
		currentPiece.warpToPlayAreaPosition();
	}

	public void togglePause(){
		if (gameIsPaused == false) {
			gameIsPaused = true;
		} else {
			gameIsPaused = false;
		}
	}
}
