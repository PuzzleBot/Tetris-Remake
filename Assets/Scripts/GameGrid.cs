using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*Object class responsible for tracking the contents of the game space
  and the cubes within it.*/
public class GameGrid : MonoBehaviour {
	private const int PLAY_AREA_HEIGHT = 34;
	private const int PLAY_AREA_WIDTH = 16;

	/*[z][x]*/
	private int[][] blockGrid;
	private int[] lowestEmptySpace;

	private GameObject[][] gridCubes;

	private int animationFramesLeft;
	private ArrayList linesToClear;

	private Tetris_Engine gameEngine;

	public void Awake() {
		int i;
		int j;

		gameEngine = GameObject.Find ("GameRuleEngine").GetComponent<Tetris_Engine> ();
		if (gameEngine == null) {
			Debug.Log ("Error: Game grid script not found.\n");
			Application.Quit ();
		}

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

		animationFramesLeft = 0;
		linesToClear = new ArrayList();
		linesToClear.Clear ();
	}

	public void Update() {
		if (animationFramesLeft > 0) {
			animateOneFrame ();
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
					gridCubes [(int)rootedGridPositions [i].y] [(int)rootedGridPositions [i].x].name = "Rooted_Cube";
				}

				block.destroyModel ();
			}
		}

		return(collisionImminent);
	}

	public int checkForLines(TetrisBlock rootedBlock){
		int checkLowerBound = rootedBlock.getLowestOccupiedGridY ();
		int linesCleared = 0;
		bool holeFound;
		int i;
		int j;

		for (i = checkLowerBound; i < checkLowerBound + 4; i++) {
			/*Don't check outside of the play area*/
			if (i < PLAY_AREA_HEIGHT) {
				holeFound = false;
				for (j = 0; j < PLAY_AREA_WIDTH; j++) {
					if (blockGrid [i] [j] == 0) {
						/*There's a hole in the line - don't clear*/
						holeFound = true;
					}
				}

				if (holeFound == false) {
					linesCleared++;
					clearLine (i);
				}
			}
		}


		return(linesCleared);
	}

	public void clearLine(int row){
		animationFramesLeft = PLAY_AREA_WIDTH;
		linesToClear.Add (row);
		gameEngine.haltGame ();
	}

	public void animateOneFrame(){
		int i;

		/*Do this first to make array math easier*/
		animationFramesLeft--;
		linesToClear.Sort ();
		linesToClear.Reverse ();

		/*Iterate through the list of line rows to clear*/
		foreach(int lineRow in linesToClear) {
			GameObject.Destroy(gridCubes [lineRow] [animationFramesLeft]);
			gridCubes [lineRow] [animationFramesLeft] = null;

			for (i = lineRow; i < PLAY_AREA_HEIGHT; i++) {
				/*Move everything above the row down*/
				if (i == PLAY_AREA_HEIGHT - 1) {
					/*Special case: the top row becomes empty*/
					blockGrid [i] [animationFramesLeft] = 0;
					gridCubes [i] [animationFramesLeft] = null;
				} else {
					if (gridCubes [i + 1] [animationFramesLeft] != null) {
						gridCubes [i + 1] [animationFramesLeft].GetComponent<Transform> ().position = gridCubes [i + 1] [animationFramesLeft].GetComponent<Transform> ().position - new Vector3 (0, 0, 1);
					}

					blockGrid [i] [animationFramesLeft] = blockGrid [i + 1] [animationFramesLeft];
					gridCubes [i] [animationFramesLeft] = gridCubes [i + 1] [animationFramesLeft];
				}
			}
		}
			
		if (animationFramesLeft <= 0) {
			linesToClear.Clear ();
			gameEngine.unHaltGame ();
		}
	}
}
