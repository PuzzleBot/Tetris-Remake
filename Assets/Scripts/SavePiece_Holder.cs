using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePiece_Holder : Piece_Holder {
	public bool alreadySwappedOnce;

	// Use this for initialization
	void Start () {
		alreadySwappedOnce = false;
		savedBlock = null;
		blockLocation = GetComponent<Transform> ().position + new Vector3(-1, 1, -1);
	}

	TetrisBlock swapBlock(TetrisBlock block){
		TetrisBlock previousBlock = savedBlock;
		savedBlock = block;
		moveBlockToHolder (block);
		return previousBlock;
	}
}
