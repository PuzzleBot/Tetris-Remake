using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece_Holder : MonoBehaviour{
	protected TetrisBlock savedBlock;
	protected Vector3 blockLocation;

	public void putBlockInHolder(TetrisBlock block){
		block.warpTo (blockLocation);
		savedBlock = block;
	}

	public TetrisBlock getHeldBlock(){
		return savedBlock;
	}

	public void warpCurrentBlockToHolder(){
		savedBlock.warpTo (blockLocation);
	}
}
