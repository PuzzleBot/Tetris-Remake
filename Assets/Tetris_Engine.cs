using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetris_Engine : MonoBehaviour {
	private const int PLAY_AREA_HEIGHT = 30;
	private const int PLAY_AREA_WIDTH = 16;

	private int[][] blockGrid;

	private TetrisBlock currentPiece;
	private TetrisBlock nextPiece;

	private int pieceTopLeftBlockPos;
	private bool pieceIsAtBottom;

	private int gravityCounter;
	private int gravityUpdateCount;

	private int keyCounter;
	private int keyCooldown;

	// Use this for initialization
	public void Start () {
		int i;
		int j;

		/*Game area block grid initialization*/
		blockGrid = new int[PLAY_AREA_HEIGHT][];
		for (i = 0; i < PLAY_AREA_HEIGHT; i++) {
			blockGrid [i] = new int[PLAY_AREA_WIDTH];
			for (j = 0; j < PLAY_AREA_WIDTH; j++) {
				blockGrid [i] [j] = 0;
			}
		}

		/*Piece buffer initialization*/
		currentPiece = new TetrisBlock (0);
		nextPiece = new TetrisBlock (0);

		moveNextPieceToCurrent ();
		generateNextPiece ();

		gravityCounter = 0;
		gravityUpdateCount = 30;

		keyCounter = 0;
		keyCooldown = 10;

		pieceTopLeftBlockPos = 27;
		pieceIsAtBottom = false;

	}
	
	// Update is called once per frame
	public void Update () {
		gravityCounter++;

		/*Key check*/
		if (keyCounter < keyCooldown) {
			keyCounter++;
		}
		else {
			if (Input.GetKey ("a")) {
				currentPiece.shiftLeft ();
				keyCounter = 0;
			} else if (Input.GetKey ("d")) {
				currentPiece.shiftRight ();
				keyCounter = 0;
			}
		}

		/*Gravity tick per half second*/
		if (gravityCounter >= gravityUpdateCount) {
			pieceGravity ();
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

	public void pieceGravity(){
		pieceTopLeftBlockPos--;
		currentPiece.gravity ();
	}
}
