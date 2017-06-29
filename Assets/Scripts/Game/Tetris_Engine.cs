using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*Singleton class responsible for driving the game's behaviour.
  This class talks to both the GameGrid object and the two
  TetrisBlock objects to make sure the game operates properly.*/
public class Tetris_Engine : MonoBehaviour {
	/*Scripts already in the scene - block holders and the game grid*/
	private GameGrid grid;
	private NextPiece_Holder nextHolder;
	private SavePiece_Holder saveHolder;

	private TetrisBlockFactory pieceFactory;

	private TetrisBlock currentPiece;


	/*Scoring variables*/
	public int score;
	public int linesDestroyed;
	public int level;
	public bool defeat;

	/*Timer counters*/
	private int gravityCounter;
	private int gravityUpdateCount;
	private int normalGravityUpdateCount;

	private int keyCounter;
	private int keyCooldown;

	private bool gameIsPaused;
	private bool gameIsHalted;

	// Use this for initialization
	public void Start () {
		grid = GameObject.Find ("GamePanel").GetComponent<GameGrid>();
		if (grid == null) {
			Debug.Log ("Error: Game grid script not found.\n");
			Application.Quit ();
		}

		/*Get the holder scripts from the holder objects in the scene*/
		nextHolder = GameObject.Find ("Model_Nextpiece_Cage").GetComponent<NextPiece_Holder>();
		saveHolder = GameObject.Find ("Model_Hold_Cage").GetComponent<SavePiece_Holder>();

		pieceFactory = new TetrisBlockFactory();

		/*Piece buffer initialization*/
		nextHolder.generateNewPiece ();
		moveNextPieceToCurrent ();

		score = 0;
		linesDestroyed = 0;
		level = 1;
		defeat = false;

		gravityCounter = 0;
		gravityUpdateCount = 30;
		normalGravityUpdateCount = 30;

		keyCounter = 0;
		keyCooldown = 10;

		gameIsPaused = false;
		gameIsHalted = false;

		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;
	}
	
	// Update is called once per frame
	public void Update () {
		if (Input.GetKey (KeyCode.Escape)) {
			Application.Quit ();
		}

		/*Down key accelerates gravity*/
		if(Input.GetKeyDown("s")){
			gravityUpdateCount = 5;
		}
		else if (Input.GetKeyUp ("s")) {
			gravityUpdateCount = normalGravityUpdateCount;
		}

		/*Halted for animation: don't do anything*/
		if (gameIsHalted == true) {
			return;
		}

		/*Key check*/
		if (keyCounter < keyCooldown) {
			keyCounter++;
		}
		else {
			/*Game paused*/
			if(Input.GetKey("p")){
				togglePause ();
				keyCounter = 0;
			}

			if (gameIsPaused == true) {
				return;
			}

			if (Input.GetKey ("a")) {
				/*Collision check for left movement*/
				if (!grid.collision (TetrisBlock.MoveType.LEFT, currentPiece)) {
					currentPiece.shiftLeft ();
				}
				keyCounter = 0;
			} else if (Input.GetKey ("d")) {
				if (!grid.collision (TetrisBlock.MoveType.RIGHT, currentPiece)) {
					currentPiece.shiftRight ();
				}
				keyCounter = 0;
			} else if (Input.GetKey ("q")) {
				if (!grid.collision (TetrisBlock.MoveType.ROTATE_LEFT, currentPiece)) {
					currentPiece.rotateRight ();
				}
				keyCounter = 0;
			} else if (Input.GetKey ("e")) {
				if (!grid.collision (TetrisBlock.MoveType.ROTATE_RIGHT, currentPiece)) {
					currentPiece.rotateRight ();
				}
				keyCounter = 0;
			} else if (Input.GetKey ("w")) {
				/*Send the piece to the bottom*/
				grid.forcePieceToBottom (currentPiece);

				/*Do a collision check immediately*/
				gravityCounter = gravityUpdateCount + 1;
				keyCounter = 0;
			} else if (Input.GetKey(KeyCode.Space)){
				if(saveHolder.alreadySwappedOnce == false){
					swapCurrentWithSavedPiece ();
					keyCounter = 0;	
				}
			}
		}

		if (gameIsPaused == true) {
			return;
		}

		gravityCounter++;

		/*Gravity tick per half second*/
		if (gravityCounter >= gravityUpdateCount) {
			if (!grid.collision (TetrisBlock.MoveType.DOWN, currentPiece)) {
				/*Move down if there is no collision*/
				currentPiece.gravity ();
			} else {
				/*Downward collision - the piece has been rooted in place.
				  Check for filled lines to clear.*/
				linesDestroyed = linesDestroyed + grid.checkForLines (currentPiece);
				GameObject.Find ("OverlayCanvas/Model_LineText/LineCounter").GetComponent<Text> ().text = linesDestroyed.ToString();

				moveNextPieceToCurrent ();
				saveHolder.alreadySwappedOnce = false;
			}
			gravityCounter = 0;
		}
	}

	/*Moves the next piece into the play area, then generates a new piece as the next*/
	public void moveNextPieceToCurrent(){
		Vector2[] currentPieceGridPositions;

		currentPiece = pieceFactory.createNewBlock(nextHolder.getHeldBlock().getBlockType());
		currentPiece.warpToPlayAreaPosition();
		currentPieceGridPositions = currentPiece.getCurrentOccupiedGrid ();

		/*Check for lose condition: no space for the new piece*/
		int i;
		for (i = 0; i < 4; i++) {
			if(grid.isOccupied(currentPieceGridPositions[i])){
				defeat = true;
			}
		}

		if (defeat == true) {
			/*If the player has lost*/
		}

		nextHolder.generateNewPiece ();
	}

	public void swapCurrentWithSavedPiece(){
		currentPiece = saveHolder.swapBlock (currentPiece);

		if (currentPiece == null) {
			/*No piece previously saved - Put the next piece into play*/
			moveNextPieceToCurrent ();
		} else {
			/*Piece previously saved - put it into play*/
			currentPiece.warpToPlayAreaPosition ();
		}

		/*Reset the gravity timer so the player has time to react*/
		gravityCounter = 0;
	}

	public void togglePause(){
		if (gameIsPaused == false) {
			gameIsPaused = true;
			GameObject.Find ("PauseText").GetComponent<Text>().enabled = true;
		} else {
			gameIsPaused = false;
			GameObject.Find ("PauseText").GetComponent<Text>().enabled = false;
		}
	}

	public void haltGame(){
		gameIsHalted = true;
	}

	public void unHaltGame(){
		gameIsHalted = false;
	}
}
