using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPiece_Holder : Piece_Holder {
	private TetrisBlockFactory pieceFactory;

	// Use this for initialization
	void Awake () {
		savedBlock = null;
		blockLocation = GetComponent<Transform> ().position + new Vector3(-1, 1, -1);

		pieceFactory = new TetrisBlockFactory ();
	}

	/*Pick a new piece at random*/
	public void generateNewPiece(){
		if (savedBlock != null) {
			savedBlock.destroyModel ();
			savedBlock = null;
		}
		savedBlock = pieceFactory.createNewBlock (0);
		warpCurrentBlockToHolder ();
		//Debug.Log ("Piece Generated: " + savedBlock.getBlockModelPositions()[0] + ", " + savedBlock.getBlockModelPositions()[1] + ", " + savedBlock.getBlockModelPositions()[2] + ", " + savedBlock.getBlockModelPositions()[3] + ", ");
	}

}
