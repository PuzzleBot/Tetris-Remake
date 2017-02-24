using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock{
	/*Predefine position vectors for use later*/
	private static Vector3 nextPiecePosition = new Vector3 ((float)19.0, (float)11.0, (float)8.0);
	private static Vector3 playAreaPosition = new Vector3 ((float)-0.5, (float)11.0, (float)14.5);

	private const double LEFT_WALL_X = -7.5;
	private const double BOTTOM_Z = -14.5;

	/*Predefine movement vectors*/
	private static Vector3 gravityAmount = new Vector3 (0, 0, 1);
	private static Vector3 leftMoveAmount = new Vector3 (-1, 0, 0);
	private static Vector3 rightMoveAmount = new Vector3 (1, 0, 0);

	/*Preload materials for the tetris blocks*/
	private static Material lineMaterial = Resources.Load("Piece1_Mat", typeof(Material)) as Material;
	private static Material sMaterial = Resources.Load("Piece2_Mat", typeof(Material)) as Material;
	private static Material zMaterial = Resources.Load("Piece3_Mat", typeof(Material)) as Material;
	private static Material tMaterial = Resources.Load("Piece4_Mat", typeof(Material)) as Material;
	private static Material lMaterial = Resources.Load("Piece5_Mat", typeof(Material)) as Material;
	private static Material backlMaterial = Resources.Load("Piece6_Mat", typeof(Material)) as Material;
	private static Material boxMaterial = Resources.Load("Piece7_Mat", typeof(Material)) as Material;

	/*Allowed player movements for pieces*/
	public enum MoveType {LEFT, RIGHT, ROTATE_LEFT, ROTATE_RIGHT, DOWN, FORCE_DOWN};

	/*Defines a offsets from the bottom-left block, in xyz coordinates
	 *in order to shape the tetris block*/
	private Vector3[] blockConfiguration;

	private int blockType;
	private GameObject[] blockModel;

	private Vector3 bottomLeftBlockPosition;
	private int rotateState;

	/* Create a new tetris block of a given type, and place it in the "next piece" area
	 * - use type 0 for a random piece*/
	public TetrisBlock(int blockType){
		int i;

		blockConfiguration = new Vector3[4];
		for (i = 0; i < 4; i++) {
			blockConfiguration [i] = new Vector3 ();
		}

		blockModel = new GameObject[4];
		for (i = 0; i < 4; i++) {
			blockModel [i] = GameObject.CreatePrimitive (PrimitiveType.Cube);
		}

		/*Set the type of block - square, L piece, line piece, etc*/
		if (blockType == 0) {
			changeToRandom ();
		} else {
			changeType (blockType);
		}

		rotateState = 0;

		bottomLeftBlockPosition = new Vector3();
		warpToNextPiecePosition ();
	}

	public void clear(){
		blockType = 0;
	}

	public int getBlockType(){
		return blockType;
	}

	public Material getBlockMaterial(){
		switch (blockType) {
		case 1:
			return(lineMaterial);
		case 2:
			return(sMaterial);
		case 3:
			return(zMaterial);
		case 4:
			return(tMaterial);
		case 5:
			return(lMaterial);
		case 6:
			return(backlMaterial);
		case 7:
			return(boxMaterial);
		default:
			return(lineMaterial);
		}
	}

	public void changeType(int newType){
		this.blockType = newType;
		switch (newType) {
		case 1:
			/*Line*/
			blockConfiguration [0].Set (1, 0, 0);
			blockConfiguration [1].Set (1, 0, 1);
			blockConfiguration [2].Set (1, 0, 2);
			blockConfiguration [3].Set (1, 0, 3);
			blockModel [0].GetComponent<Renderer> ().material = lineMaterial;
			blockModel [1].GetComponent<Renderer> ().material = lineMaterial;
			blockModel [2].GetComponent<Renderer> ().material = lineMaterial;
			blockModel [3].GetComponent<Renderer> ().material = lineMaterial;
			break;
		case 2:
			/* S */
			blockConfiguration [0].Set(0, 0, 0);
			blockConfiguration [1].Set(1, 0, 0);
			blockConfiguration [2].Set(1, 0, 1);
			blockConfiguration [3].Set(2, 0, 1);
			blockModel [0].GetComponent<Renderer> ().material = sMaterial;
			blockModel [1].GetComponent<Renderer> ().material = sMaterial;
			blockModel [2].GetComponent<Renderer> ().material = sMaterial;
			blockModel [3].GetComponent<Renderer> ().material = sMaterial;
			break;
		case 3:
			/* Z */
			blockConfiguration [0].Set(0, 0, 1);
			blockConfiguration [1].Set(1, 0, 1);
			blockConfiguration [2].Set(1, 0, 0);
			blockConfiguration [3].Set(2, 0, 0);
			blockModel [0].GetComponent<Renderer> ().material = zMaterial;
			blockModel [1].GetComponent<Renderer> ().material = zMaterial;
			blockModel [2].GetComponent<Renderer> ().material = zMaterial;
			blockModel [3].GetComponent<Renderer> ().material = zMaterial;
			break;
		case 4:
			/* T */
			blockConfiguration [0].Set(0, 0, 0);
			blockConfiguration [1].Set(1, 0, 0);
			blockConfiguration [2].Set(1, 0, 1);
			blockConfiguration [3].Set(2, 0, 0);
			blockModel [0].GetComponent<Renderer> ().material = tMaterial;
			blockModel [1].GetComponent<Renderer> ().material = tMaterial;
			blockModel [2].GetComponent<Renderer> ().material = tMaterial;
			blockModel [3].GetComponent<Renderer> ().material = tMaterial;
			break;
		case 5:
			/* L */
			blockConfiguration [0].Set(1, 0, 0);
			blockConfiguration [1].Set(2, 0, 0);
			blockConfiguration [2].Set(1, 0, 1);
			blockConfiguration [3].Set(1, 0, 2);
			blockModel [0].GetComponent<Renderer> ().material = lMaterial;
			blockModel [1].GetComponent<Renderer> ().material = lMaterial;
			blockModel [2].GetComponent<Renderer> ().material = lMaterial;
			blockModel [3].GetComponent<Renderer> ().material = lMaterial;
			break;
		case 6:
			/*Backwards L*/
			blockConfiguration [0].Set(0, 0, 0);
			blockConfiguration [1].Set(1, 0, 0);
			blockConfiguration [2].Set(1, 0, 1);
			blockConfiguration [3].Set(1, 0, 2);
			blockModel [0].GetComponent<Renderer> ().material = backlMaterial;
			blockModel [1].GetComponent<Renderer> ().material = backlMaterial;
			blockModel [2].GetComponent<Renderer> ().material = backlMaterial;
			blockModel [3].GetComponent<Renderer> ().material = backlMaterial;
			break;
		case 7:
			/*2x2 block*/
			blockConfiguration [0].Set(1, 0, 0);
			blockConfiguration [1].Set(1, 0, 1);
			blockConfiguration [2].Set(2, 0, 0);
			blockConfiguration [3].Set(2, 0, 1);
			blockModel [0].GetComponent<Renderer> ().material = boxMaterial;
			blockModel [1].GetComponent<Renderer> ().material = boxMaterial;
			blockModel [2].GetComponent<Renderer> ().material = boxMaterial;
			blockModel [3].GetComponent<Renderer> ().material = boxMaterial;
			break;
		default:
			/*Default to a line*/
			blockConfiguration [0].Set(1, 0, 0);
			blockConfiguration [1].Set(1, 0, 1);
			blockConfiguration [2].Set(1, 0, 2);
			blockConfiguration [3].Set(1, 0, 3);
			blockModel [0].GetComponent<Renderer> ().material = lineMaterial;
			blockModel [1].GetComponent<Renderer> ().material = lineMaterial;
			blockModel [2].GetComponent<Renderer> ().material = lineMaterial;
			blockModel [3].GetComponent<Renderer> ().material = lineMaterial;
			break;
		}

		blockModel [0].transform.position = bottomLeftBlockPosition + blockConfiguration[0];
		blockModel [1].transform.position = bottomLeftBlockPosition + blockConfiguration[1];
		blockModel [2].transform.position = bottomLeftBlockPosition + blockConfiguration[2];
		blockModel [3].transform.position = bottomLeftBlockPosition + blockConfiguration[3];
	}

	/*Changes this piece to a different random one*/
	public void changeToRandom(){
		blockType = UnityEngine.Random.Range (1, 8);
		changeType (blockType);
	}

	/*Teleport this piece to the specified coordinates*/
	public void warpTo(float xpos, float ypos, float zpos){
		bottomLeftBlockPosition.Set(xpos, ypos, zpos);
		blockModel [0].transform.position = bottomLeftBlockPosition + blockConfiguration[0];
		blockModel [1].transform.position = bottomLeftBlockPosition + blockConfiguration[1];
		blockModel [2].transform.position = bottomLeftBlockPosition + blockConfiguration[2];
		blockModel [3].transform.position = bottomLeftBlockPosition + blockConfiguration[3];
	}

	public void warpTo(Vector3 position){
		bottomLeftBlockPosition.Set(position.x, position.y, position.z);
		blockModel [0].transform.position = bottomLeftBlockPosition + blockConfiguration[0];
		blockModel [1].transform.position = bottomLeftBlockPosition + blockConfiguration[1];
		blockModel [2].transform.position = bottomLeftBlockPosition + blockConfiguration[2];
		blockModel [3].transform.position = bottomLeftBlockPosition + blockConfiguration[3];
	}

	/*Teleport this piece into the "next piece" box*/
	public void warpToNextPiecePosition(){
		bottomLeftBlockPosition.Set(nextPiecePosition.x, nextPiecePosition.y, nextPiecePosition.z);
		blockModel [0].transform.position = blockConfiguration [0] + nextPiecePosition;
		blockModel [1].transform.position = blockConfiguration [1] + nextPiecePosition;
		blockModel [2].transform.position = blockConfiguration [2] + nextPiecePosition;
		blockModel [3].transform.position = blockConfiguration [3] + nextPiecePosition;
	}

	/*Teleport this piece into the play area, where all the pieces start*/
	public void warpToPlayAreaPosition(){
		bottomLeftBlockPosition.Set(playAreaPosition.x, playAreaPosition.y, playAreaPosition.z);
		blockModel [0].transform.position = blockConfiguration [0] + playAreaPosition;
		blockModel [1].transform.position = blockConfiguration [1] + playAreaPosition;
		blockModel [2].transform.position = blockConfiguration [2] + playAreaPosition;
		blockModel [3].transform.position = blockConfiguration [3] + playAreaPosition;
	}

	/*Move the piece down based on gravity*/
	public void gravity(){
		warpTo (bottomLeftBlockPosition - gravityAmount);
	}

	/*Use when the left key is pushed*/
	public void shiftLeft(){
		warpTo (bottomLeftBlockPosition + leftMoveAmount);
	}

	/*Use when the right key is pushed*/
	public void shiftRight(){
		warpTo (bottomLeftBlockPosition + rightMoveAmount);
	}

	/*Convert the model's current 3D vector coordinates into 2D grid coordinates*/
	public Vector2[] getCurrentOccupiedGrid(){
		Vector2[] occupiedSquares = new Vector2[4];
		int i;

		/*Transform the x and z coordinates so that the leftmost x and bottommost z are zero*/
		for (i = 0; i < 4; i++) {
			occupiedSquares [i] = new Vector2 ((int)System.Math.Round(bottomLeftBlockPosition.x + blockConfiguration[i].x - LEFT_WALL_X), 
				(int)System.Math.Round(bottomLeftBlockPosition.z + blockConfiguration[i].z - BOTTOM_Z));
		}

		return(occupiedSquares);
	}

	/*Get the positions of the blocks in the visual model*/
	public Vector3[] getBlockModelPositions(){
		Vector3[] positions = new Vector3[4];
		int i;

		for (i = 0; i < 4; i++) {
			positions [i] = blockModel [i].GetComponent<Transform> ().position;
		}

		return(positions);
	}


	/*Convert the model's 3D vector predicted coordinates after a movement into 2D grid coordinates*/
	public Vector2[] calculateOccupiedGrid(MoveType movement){
		Vector3[] adjustedPositions = new Vector3[4];
		Vector2[] occupiedSquares = new Vector2[4];

		int i;

		switch (movement) {
		case MoveType.LEFT:
			adjustedPositions [0] = bottomLeftBlockPosition + blockConfiguration [0] + leftMoveAmount;
			adjustedPositions [1] = bottomLeftBlockPosition + blockConfiguration [1] + leftMoveAmount;
			adjustedPositions [2] = bottomLeftBlockPosition + blockConfiguration [2] + leftMoveAmount;
			adjustedPositions [3] = bottomLeftBlockPosition + blockConfiguration [3] + leftMoveAmount;
			break;
		case MoveType.RIGHT:
			adjustedPositions [0] = bottomLeftBlockPosition + blockConfiguration[0] + rightMoveAmount;
			adjustedPositions [1] = bottomLeftBlockPosition + blockConfiguration[1] + rightMoveAmount;
			adjustedPositions [2] = bottomLeftBlockPosition + blockConfiguration[2] + rightMoveAmount;
			adjustedPositions [3] = bottomLeftBlockPosition + blockConfiguration[3] + rightMoveAmount;
			break;
		case MoveType.DOWN:
			adjustedPositions [0] = bottomLeftBlockPosition + blockConfiguration [0] - gravityAmount;
			adjustedPositions [1] = bottomLeftBlockPosition + blockConfiguration [1] - gravityAmount;
			adjustedPositions [2] = bottomLeftBlockPosition + blockConfiguration [2] - gravityAmount;
			adjustedPositions [3] = bottomLeftBlockPosition + blockConfiguration [3] - gravityAmount;
			break;
		default:
			adjustedPositions [0] = bottomLeftBlockPosition + blockConfiguration [0] - gravityAmount;
			adjustedPositions [1] = bottomLeftBlockPosition + blockConfiguration [1] - gravityAmount;
			adjustedPositions [2] = bottomLeftBlockPosition + blockConfiguration [2] - gravityAmount;
			adjustedPositions [3] = bottomLeftBlockPosition + blockConfiguration [3] - gravityAmount;
			Debug.Log ("Warning: Defaulting to downward movement.");
			break;
		}

		/*Transform the x and z coordinates so that the leftmost x and bottommost z are zero*/
		for (i = 0; i < 4; i++) {
			occupiedSquares [i] = new Vector2 ((int)System.Math.Round(adjustedPositions [i].x - LEFT_WALL_X), 
											   (int)System.Math.Round(adjustedPositions [i].z - BOTTOM_Z));
		}

		return(occupiedSquares);
	}
}
