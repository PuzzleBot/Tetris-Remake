﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
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

	/*Keybinding controls*/
	private KeyBindings keyBindings;

	/*Scoring variables*/
	public GameScore scoreRecords;
	public int linesDestroyed;
	public bool defeat;

	/*Timer counters*/
	private int gravityCounter;
	private int gravityUpdateCount;
	private int normalGravityUpdateCount;

	private int keyCounter;
	private int keyCooldown;

	private bool gameIsPaused;
	private bool gameIsHalted;

	/*UI text references stored for later*/
	private GameObject pauseMenu;
	private GameObject lineCountText;
	private GameObject levelText;

	private GameObject mainOverlay;
	private GameObject defeatOverlay;
	private GameObject finalScoreText;

	// Use this for initialization
	public void Start () {
		grid = GameObject.Find ("GamePanel").GetComponent<GameGrid>();
		if (grid == null) {
			Debug.Log ("Error: Game grid script not found.\n");
			Application.Quit ();
		}

		/*Debug.Log ("Deleting all preferences");
		PlayerPrefs.DeleteAll ();*/

		/*Get the holder scripts from the holder objects in the scene*/
		nextHolder = GameObject.Find ("Model_Nextpiece_Cage").GetComponent<NextPiece_Holder>();
		saveHolder = GameObject.Find ("Model_Hold_Cage").GetComponent<SavePiece_Holder>();

		pieceFactory = new TetrisBlockFactory();

		/*Piece buffer initialization*/
		nextHolder.generateNewPiece ();
		moveNextPieceToCurrent ();

		keyBindings = KeyBindings.getInstance ();

		scoreRecords = new GameScore();
		linesDestroyed = 0;
		defeat = false;

		gravityCounter = 0;
		gravityUpdateCount = 30;
		normalGravityUpdateCount = 30;

		keyCounter = 0;
		keyCooldown = 10;

		gameIsPaused = false;
		gameIsHalted = false;

		pauseMenu = GameObject.Find("OverlayUI/OverlayCanvas/Model_PauseMenu");
		lineCountText = GameObject.Find ("OverlayUI/OverlayCanvas/Model_ScoreText/ScoreCounter");
		levelText = GameObject.Find ("OverlayUI/OverlayCanvas/LevelText");

		mainOverlay = GameObject.Find("OverlayUI/OverlayCanvas");
		defeatOverlay = GameObject.Find("OverlayUI/DefeatOverlayCanvas");
		finalScoreText = GameObject.Find ("OverlayUI/DefeatOverlayCanvas/FinalScoreText");

		defeatOverlay.SetActive (false);
		pauseMenu.SetActive (false);

		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;

		Debug.Log (Application.persistentDataPath);
	}
	
	// Update is called once per frame
	public void Update () {
		if (Input.GetKey (KeyCode.Escape)) {
			Application.Quit ();
		}

		/*Halted for animation, or in a menu: don't do anything*/
		if (gameIsHalted == true) {
			return;
		}

		/*Accelerate gravity*/
		if(Input.GetKeyDown(keyBindings.getBoundKey("AccelerateDown"))){
			gravityUpdateCount = 3;
		}
		else if (Input.GetKeyUp (keyBindings.getBoundKey("AccelerateDown"))) {
			gravityUpdateCount = normalGravityUpdateCount;
		}

		/*Key check*/
		if (keyCounter < keyCooldown) {
			keyCounter++;
		}
		else {
			/*Game paused*/
			if(Input.GetKey(keyBindings.getBoundKey("Pause"))){
				togglePause ();
				keyCounter = 0;
			}

			if (gameIsPaused == true) {
				return;
			}

			if (Input.GetKey (keyBindings.getBoundKey("MoveLeft"))) {
				/*Collision check for left movement*/
				if (!grid.collision (TetrisBlock.MoveType.LEFT, currentPiece)) {
					currentPiece.shiftLeft ();
				}
				keyCounter = 0;
			} else if (Input.GetKey (keyBindings.getBoundKey("MoveRight"))) {
				/*Right Movement*/
				if (!grid.collision (TetrisBlock.MoveType.RIGHT, currentPiece)) {
					currentPiece.shiftRight ();
				}
				keyCounter = 0;
			} else if (Input.GetKey (keyBindings.getBoundKey("RotateCounterClockwise"))) {
				if (!grid.collision (TetrisBlock.MoveType.ROTATE_LEFT, currentPiece)) {
					currentPiece.rotateRight ();
				}
				keyCounter = 0;
			} else if (Input.GetKey (keyBindings.getBoundKey("RotateClockwise"))) {
				if (!grid.collision (TetrisBlock.MoveType.ROTATE_RIGHT, currentPiece)) {
					currentPiece.rotateRight ();
				}
				keyCounter = 0;
			} else if (Input.GetKey (keyBindings.getBoundKey("ForceDown"))) {
				/*Send the piece to the bottom*/
				grid.forcePieceToBottom (currentPiece);

				/*Do a collision check immediately*/
				gravityCounter = gravityUpdateCount + 1;
				keyCounter = 0;
			} else if (Input.GetKey(keyBindings.getBoundKey("SavePiece"))){
				if(saveHolder.alreadySwappedOnce == false){
					swapCurrentWithSavedPiece ();
					keyCounter = 0;	
				}
			} else if (Input.GetKey (KeyCode.Comma)) {
				/*Debug key "," - increment level*/
				scoreRecords.incrementLevel ();
				levelText.GetComponent<Text> ().text = "Level " + scoreRecords.getLevel().ToString();
				updateLevelGravity(scoreRecords.getLevel());
				keyCounter = 0;
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
				linesDestroyed = grid.checkForLines (currentPiece);
				scoreRecords.addLineClearScore (linesDestroyed);
				lineCountText.GetComponent<Text> ().text = scoreRecords.getScoreValue().ToString();
				levelText.GetComponent<Text> ().text = "Level " + scoreRecords.getLevel().ToString();
				updateLevelGravity(scoreRecords.getLevel());

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
			/*If the player has lost, tell the play area to start the defeat animation*/
			haltGame ();
			grid.signalDefeat ();
		} else {
			nextHolder.generateNewPiece ();
		}
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
		/*Don't allow pausing if the defeat screen is up*/
		if (defeat == true) {
			return;
		}

		if (gameIsPaused == false) {
			gameIsPaused = true;
			pauseMenu.SetActive(true);
		} else {
			gameIsPaused = false;
			pauseMenu.SetActive(false);
		}
	}

	public void haltGame(){
		gameIsHalted = true;
	}

	public void unHaltGame(){
		gameIsHalted = false;
	}

	public void activateDefeatOverlay(){
		defeatOverlay.SetActive(true);
		mainOverlay.SetActive(false);

		finalScoreText.GetComponent<Text> ().text = "Final Score: " + scoreRecords.getScoreValue().ToString ();
		defeatOverlay.GetComponent<ScoreEnter_Engine>().setFinalScore(scoreRecords.getScoreValue());
	}

	public void updateLevelGravity(int newLevel){
		setGravity (30 - ((newLevel) * 2));
	}

	private void setGravity(int newGravityUpdateCount){
		if (newGravityUpdateCount < 3) {
			normalGravityUpdateCount = 3;
			gravityUpdateCount = normalGravityUpdateCount;
		} else {
			normalGravityUpdateCount = newGravityUpdateCount;
			gravityUpdateCount = normalGravityUpdateCount;
		}
	}
}
