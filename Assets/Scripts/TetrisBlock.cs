using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*Object class representing a tetris piece.*/
public abstract class TetrisBlock{
	/*Predefine position vectors for use later*/
	private static Vector3 nextPiecePosition = new Vector3 ((float)19.0, (float)11.0, (float)8.0);
	private static Vector3 playAreaPosition = new Vector3 ((float)-0.5, (float)11.0, (float)14.5);

	private const double LEFT_WALL_X = -7.55;
	private const double BOTTOM_Z = -14.55;

	/*Predefine movement vectors*/
	private static Vector3 gravityAmount = new Vector3 (0, 0, 1);
	private static Vector3 leftMoveAmount = new Vector3 (-1, 0, 0);
	private static Vector3 rightMoveAmount = new Vector3 (1, 0, 0);

	/*Allowed player movements for pieces*/
	public enum MoveType {LEFT, RIGHT, ROTATE_LEFT, ROTATE_RIGHT, DOWN, FORCE_DOWN};

	protected int maxRotationStates;

	/*Defines a offsets from the bottom-left block, in xyz coordinates
	 *in order to shape the tetris block*/
	protected Vector3[] blockConfiguration;

	protected int blockType;
	protected GameObject[] blockModel;

	protected Vector3 bottomLeftBlockPosition;
	protected int rotateState;

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

		rotateState = 0;
		maxRotationStates = 1;
		this.blockType = blockType;

		bottomLeftBlockPosition = new Vector3();
	}

	public int getBlockType(){
		return blockType;
	}

	public abstract Material getBlockMaterial ();

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

	public void rotateLeft(){
		blockConfiguration = getPreviousRotationConfiguration ();
		blockModel [0].transform.position = bottomLeftBlockPosition + blockConfiguration[0];
		blockModel [1].transform.position = bottomLeftBlockPosition + blockConfiguration[1];
		blockModel [2].transform.position = bottomLeftBlockPosition + blockConfiguration[2];
		blockModel [3].transform.position = bottomLeftBlockPosition + blockConfiguration[3];
		rotateState = System.Math.Abs(rotateState - 1) % maxRotationStates;
	}

	public void rotateRight(){
		blockConfiguration = getNextRotationConfiguration ();
		blockModel [0].transform.position = bottomLeftBlockPosition + blockConfiguration[0];
		blockModel [1].transform.position = bottomLeftBlockPosition + blockConfiguration[1];
		blockModel [2].transform.position = bottomLeftBlockPosition + blockConfiguration[2];
		blockModel [3].transform.position = bottomLeftBlockPosition + blockConfiguration[3];
		rotateState = (rotateState + 1) % maxRotationStates;
	}

	/*Convert the model's current 3D vector coordinates into 2D grid coordinates*/
	public Vector2[] getCurrentOccupiedGrid(){
		Vector2[] occupiedSquares = new Vector2[4];
		int i;

		/*Transform the x and z coordinates so that the leftmost x and bottommost z are zero*/
		for (i = 0; i < 4; i++) {
			occupiedSquares [i] = new Vector2 ((int)(System.Math.Round(bottomLeftBlockPosition.x + blockConfiguration[i].x - LEFT_WALL_X)), 
				(int)(System.Math.Round(bottomLeftBlockPosition.z + blockConfiguration[i].z - BOTTOM_Z)));
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

	public int getLowestOccupiedGridY(){
		int i;
		double lowestY = 100;

		for(i = 0; i < 4; i++){
			if (blockModel[i].GetComponent<Transform>().position.z < lowestY) {
				lowestY = blockModel [i].GetComponent<Transform> ().position.z;
			}
		}

		return((int)(System.Math.Round(lowestY - BOTTOM_Z)));
	}


	/*Convert the model's 3D vector predicted coordinates after a movement into 2D grid coordinates*/
	public Vector2[] calculateOccupiedGrid(MoveType movement){
		Vector3[] adjustedPositions = new Vector3[4];
		Vector3[] rotationConfiguration;
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
		case MoveType.ROTATE_LEFT:
			rotationConfiguration = getPreviousRotationConfiguration ();
			adjustedPositions [0] = bottomLeftBlockPosition + rotationConfiguration [0];
			adjustedPositions [1] = bottomLeftBlockPosition + rotationConfiguration [1];
			adjustedPositions [2] = bottomLeftBlockPosition + rotationConfiguration [2];
			adjustedPositions [3] = bottomLeftBlockPosition + rotationConfiguration [3];
			break;
		case MoveType.ROTATE_RIGHT:
			rotationConfiguration = getNextRotationConfiguration ();
			adjustedPositions [0] = bottomLeftBlockPosition + rotationConfiguration [0];
			adjustedPositions [1] = bottomLeftBlockPosition + rotationConfiguration [1];
			adjustedPositions [2] = bottomLeftBlockPosition + rotationConfiguration [2];
			adjustedPositions [3] = bottomLeftBlockPosition + rotationConfiguration [3];
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
			occupiedSquares [i] = new Vector2 ((int)(System.Math.Round(adjustedPositions [i].x - LEFT_WALL_X)), 
											   (int)(System.Math.Round(adjustedPositions [i].z - BOTTOM_Z)));
		}

		return(occupiedSquares);
	}

	public abstract Vector3[] getNextRotationConfiguration ();
	public abstract Vector3[] getPreviousRotationConfiguration ();

	/*Deletes the visual model of the block.*/
	public void destroyModel(){
		int i;

		for (i = 0; i < 4; i++) {
			GameObject.Destroy (blockModel [i]);
		}
	}
}
