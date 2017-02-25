using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Singleton class responsible for driving the game's behaviour.
  This class talks to both the GameGrid object and the two
  TetrisBlock objects to make sure the game operates properly.*/
public class Tetris_Engine : MonoBehaviour {
	private GameGrid grid;

	private TetrisBlockFactory pieceFactory;

	private TetrisBlock currentPiece;
	private TetrisBlock nextPiece;

	/*Scoring variables*/
	int score;
	int linesDestroyed;
	int level;

	/*Timer counters*/
	private int gravityCounter;
	private int gravityUpdateCount;

	private int keyCounter;
	private int keyCooldown;

	private bool gameIsPaused;

	// Use this for initialization
	public void Start () {
		grid = new GameGrid ();
		pieceFactory = new TetrisBlockFactory();

		/*Piece buffer initialization*/
		nextPiece = pieceFactory.createNewBlock(0);
		moveNextPieceToCurrent ();
		nextPiece = pieceFactory.createNewBlock(0);

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
			} else if (Input.GetKey("q")){
				if(!grid.collision(TetrisBlock.MoveType.ROTATE_LEFT, currentPiece)){
					currentPiece.rotateRight ();
				}
				keyCounter = 0;
			} else if (Input.GetKey("e")){
				if(!grid.collision(TetrisBlock.MoveType.ROTATE_RIGHT, currentPiece)){
					currentPiece.rotateRight ();
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
				/*Downward collision - the piece has been rooted in place.
				  Check for filled lines to clear.*/
				linesDestroyed = grid.checkForLines (currentPiece);

				moveNextPieceToCurrent ();
				generateNextPiece ();
			}
			gravityCounter = 0;
		}
	}

	/*Pick a new piece at random*/
	public void generateNextPiece(){
		nextPiece.destroyModel();
		nextPiece = pieceFactory.createNewBlock (0);
	}

	public void moveNextPieceToCurrent(){
		currentPiece = pieceFactory.createNewBlock(nextPiece.getBlockType());
		currentPiece.warpToPlayAreaPosition();
		nextPiece.destroyModel ();
	}

	public void togglePause(){
		if (gameIsPaused == false) {
			gameIsPaused = true;
		} else {
			gameIsPaused = false;
		}
	}
}
