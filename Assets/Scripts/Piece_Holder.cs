using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece_Holder : MonoBehaviour{
	public TetrisBlock savedBlock;
	protected Vector3 blockLocation;

	public void moveBlockToHolder(TetrisBlock block){
		block.warpTo (blockLocation);
		savedBlock = block;
	}
}
