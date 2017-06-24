using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece_Holder : MonoBehaviour{
	public TetrisBlock savedBlock;
	protected Vector3 blockLocation;

	public void moveBlockToHolder(TetrisBlock block){
		if (savedBlock != null) {
			savedBlock.destroyModel();
			savedBlock = null;
		}

		block.warpTo (blockLocation);
		savedBlock = block;
	}

	public void warpCurrentBlockToHolder(){
		savedBlock.warpTo (blockLocation);
	}
}
