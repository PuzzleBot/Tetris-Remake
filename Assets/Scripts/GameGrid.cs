using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid{
	private const int PLAY_AREA_HEIGHT = 34;
	private const int PLAY_AREA_WIDTH = 16;

	/*[z][x]*/
	private int[][] blockGrid;
	private int[] lowestEmptySpace;

	private GameObject[][] gridCubes;

	public GameGrid(){
		int i;
		int j;

		/*Create the game grid for tracking collisions*/
		blockGrid = new int[PLAY_AREA_HEIGHT][];
		gridCubes = new GameObject[PLAY_AREA_HEIGHT][];
		for (i = 0; i < PLAY_AREA_HEIGHT; i++) {
			blockGrid[i] = new int [PLAY_AREA_WIDTH];
			gridCubes [i] = new GameObject[PLAY_AREA_WIDTH];
			for (j = 0; j < PLAY_AREA_WIDTH; j++) {
				blockGrid [i] [j] = 0;
				gridCubes [i] [j] = null;
			}
		}

		lowestEmptySpace = new int[PLAY_AREA_WIDTH];
		for (j = 0; j < PLAY_AREA_WIDTH; j++) {
			lowestEmptySpace [j] = 0;
		}
	}

	public bool collision(TetrisBlock.MoveType movementType, TetrisBlock block){
		/*Get the new grid coordinates, then determine if there is something else there*/
		Vector2[] blockGridPositions = block.calculateOccupiedGrid (movementType);
		bool collisionImminent = false;
		int i = 0;

		while((i < 4) && (collisionImminent == false)){
			/*Check for out of bounds or a block occupying the space the block is moving to*/
			if((blockGridPositions[i].y < 0) || (blockGridPositions[i].y >= PLAY_AREA_HEIGHT) ||
				((blockGridPositions[i].x < 0) || (blockGridPositions[i].x >= PLAY_AREA_WIDTH))){
				collisionImminent = true;
			}
			else if(blockGrid[(int)blockGridPositions[i].y][(int)blockGridPositions[i].x] != 0){
				collisionImminent = true;
			}
			i++;
		}

		if (collisionImminent){
			/*Collision - do nothing unless its a downward collsion, in which the piece should be rooted in place 
			  (i.e. the grid should be updated), and the player's control should be directed to the next piece*/
			if (movementType == TetrisBlock.MoveType.DOWN) {
				Vector2[] rootedGridPositions = block.getCurrentOccupiedGrid ();
				Vector3[] cubeModelPositions = block.getBlockModelPositions ();
				Material fillinMaterial = block.getBlockMaterial ();

				for (i = 0; i < 4; i++) {
					/*Update the game grid, and put in the actual blocks*/
					blockGrid [(int)rootedGridPositions [i].y] [(int)rootedGridPositions [i].x] = 1;
					gridCubes [(int)rootedGridPositions [i].y] [(int)rootedGridPositions [i].x] = GameObject.CreatePrimitive (PrimitiveType.Cube);
					gridCubes [(int)rootedGridPositions [i].y] [(int)rootedGridPositions [i].x].GetComponent<Transform> ().position = cubeModelPositions[i];
					gridCubes [(int)rootedGridPositions [i].y] [(int)rootedGridPositions [i].x].GetComponent<Renderer> ().material = fillinMaterial;
				}
			}
		}

		return(collisionImminent);
	}

	public int checkForLines(){
		return(0);
	}
}
