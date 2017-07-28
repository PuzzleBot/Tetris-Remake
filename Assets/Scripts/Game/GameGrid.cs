using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*Object class responsible for tracking the contents of the game space
  and the cubes within it.*/
public class GameGrid : MonoBehaviour {
	private const int PLAY_AREA_HEIGHT = 34;
	private const int PLAY_AREA_WIDTH = 10;

	private static Material defeatMaterial;

	/*[z][x]
	 *Values in this array:
	 *0 means empty space
	 *1 means space occupied by some landed tetris piece
	 *2 means space occupied by a grey cube generated on defeat*/
	private int[][] blockGrid;

	/*Last empty space before the first non-empty space*/
	private int[] lowestEmptySpace;

	private GameObject[][] gridCubes;

	private int lineAnimationFramesLeft;
	private ArrayList linesToClear;
	private bool animateDefeat;
	private Vector2 currentGreyBlock;

	private int animationTimer;
	private int animationDelay;

	private Tetris_Engine gameEngine;

	public void Awake() {
		int i;
		int j;

		defeatMaterial = Resources.Load("Defeat_Mat", typeof(Material)) as Material;

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

		lineAnimationFramesLeft = 0;
		linesToClear = new ArrayList();
		linesToClear.Clear ();
		animateDefeat = false;
		currentGreyBlock = new Vector2 (0, 0);

		animationTimer = 0;
		animationDelay = 2;
	}

	public void Update() {
		if (animationTimer >= animationDelay) {
			if (lineAnimationFramesLeft > 0) {
				animateOneLineFrame ();
				animationTimer = 0;
			}
			if (animateDefeat == true) {
				animateOneDefeatFrame ();
				animationTimer = 0;
			}
		} else {
			animationTimer++;
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
		lineAnimationFramesLeft = PLAY_AREA_WIDTH;
		linesToClear.Add (row);
		gameEngine.haltGame ();
	}

	public void animateOneLineFrame(){
		int i;

		/*Do this first to make array math easier*/
		lineAnimationFramesLeft--;
		linesToClear.Sort ();
		linesToClear.Reverse ();

		/*Iterate through the list of line rows to clear*/
		foreach(int lineRow in linesToClear) {
			GameObject.Destroy(gridCubes [lineRow] [lineAnimationFramesLeft]);
			gridCubes [lineRow] [lineAnimationFramesLeft] = null;

			for (i = lineRow; i < PLAY_AREA_HEIGHT; i++) {
				/*Move everything above the row down*/
				if (i == PLAY_AREA_HEIGHT - 1) {
					/*Special case: the top row becomes empty*/
					blockGrid [i] [lineAnimationFramesLeft] = 0;
					gridCubes [i] [lineAnimationFramesLeft] = null;
				} else {
					if (gridCubes [i + 1] [lineAnimationFramesLeft] != null) {
						gridCubes [i + 1] [lineAnimationFramesLeft].GetComponent<Transform> ().position = gridCubes [i + 1] [lineAnimationFramesLeft].GetComponent<Transform> ().position - new Vector3 (0, 0, 1);
					}

					blockGrid [i] [lineAnimationFramesLeft] = blockGrid [i + 1] [lineAnimationFramesLeft];
					gridCubes [i] [lineAnimationFramesLeft] = gridCubes [i + 1] [lineAnimationFramesLeft];
				}
			}
		}
			
		if (lineAnimationFramesLeft <= 0) {
			linesToClear.Clear ();
			gameEngine.unHaltGame ();
		}
	}


	public void animateOneDefeatFrame(){
		/*Find the next block to turn grey*/
		while(((int)System.Math.Round(currentGreyBlock.y) < PLAY_AREA_HEIGHT) && (blockGrid[(int)System.Math.Round(currentGreyBlock.y)][(int)System.Math.Round(currentGreyBlock.x)] != 1)){
			currentGreyBlock.x++;
			if ((int)System.Math.Round (currentGreyBlock.x) >= PLAY_AREA_WIDTH) {
				currentGreyBlock.x = 0;
				currentGreyBlock.y++;
			}
		}

		/*If there are no blocks left to grey out, stop the animation and show the defeat screen*/
		if ((int)System.Math.Round (currentGreyBlock.y) >= PLAY_AREA_HEIGHT) {
			endOfDefeatAnimation ();
		} else {
			gridCubes [(int)System.Math.Round (currentGreyBlock.y)] [(int)System.Math.Round (currentGreyBlock.x)].GetComponent<Renderer> ().material = defeatMaterial;
			blockGrid[(int)System.Math.Round(currentGreyBlock.y)][(int)System.Math.Round(currentGreyBlock.x)] = 2;
		}
	}

	public void forcePieceToBottom(TetrisBlock block){
		int[] blockDistances = new int[4];
		Vector2[] blockGridPositions = block.getCurrentOccupiedGrid();
		int lowestDistance = 300;
		int lowestBlockIndex = 0;
		int i;
		int currentHeight;


		/*Calculate distance from each block's current height to the next solid block below it, 
		 *not counting the solid blocks, and determine the lowest distance*/
		for (i = 0; i < 4; i++) {
			currentHeight = (int)blockGridPositions [i].y - 1;
			while((currentHeight >= 0) && (blockGrid[currentHeight][(int)blockGridPositions[i].x] == 0)){
				currentHeight--;
			}
			blockDistances [i] = (int)blockGridPositions [i].y - currentHeight - 1;

			if(lowestDistance >= blockDistances[i]){
				lowestDistance = blockDistances [i];
				lowestBlockIndex = i;
			}
		}

		Debug.Log (block.getBlockModelPositions()[lowestBlockIndex] + new Vector3(0, 0, -lowestDistance));

		/*Move the piece down by that distance*/
		block.warpTo (block.getBottomLeftBlockPosition() + new Vector3(0, 0, -lowestDistance));
	}

	public bool isOccupied(Vector2 gridPosition){
		if (blockGrid [(int)System.Math.Round(gridPosition.y)] [(int)System.Math.Round(gridPosition.x)] == 0) {
			return(false);
		} else {
			return(true);
		}
	}

	public void signalDefeat(){
		animateDefeat = true;
		currentGreyBlock.x = 0;
		currentGreyBlock.y = 0;
	}

	public void endOfDefeatAnimation(){
		animateDefeat = false;
		gameEngine.activateDefeatOverlay ();
	}
}
