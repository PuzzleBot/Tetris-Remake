using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePiece_Holder : Piece_Holder {
	public bool alreadySwappedOnce;

	// Use this for initialization
	void Awake () {
		alreadySwappedOnce = false;
		savedBlock = null;
		blockLocation = GetComponent<Transform> ().position + new Vector3(-1, 1, -1);
	}

	/*Puts the current piece on hold, returns what was previously on hold*/
	public TetrisBlock swapBlock(TetrisBlock block){
		TetrisBlock previousBlock = savedBlock;
		putBlockInHolder (block);
		alreadySwappedOnce = true;
		return previousBlock;
	}
}
