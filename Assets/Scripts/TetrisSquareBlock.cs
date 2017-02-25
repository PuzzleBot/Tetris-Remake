using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Square piece*/
public class TetrisSquareBlock : TetrisBlock {
	private static Material blockMaterial = Resources.Load("Piece7_Mat", typeof(Material)) as Material;
	private static int maxRotationStates = 1;

	private Vector3[] rotationConfiguration;

	public TetrisSquareBlock() : base(7) {
		blockModel [0].GetComponent<Renderer> ().material = blockMaterial;
		blockModel [1].GetComponent<Renderer> ().material = blockMaterial;
		blockModel [2].GetComponent<Renderer> ().material = blockMaterial;
		blockModel [3].GetComponent<Renderer> ().material = blockMaterial;

		rotationConfiguration = new Vector3[4];
		rotationConfiguration [0].Set (0, 0, 0);
		rotationConfiguration [1].Set (1, 0, 0);
		rotationConfiguration [2].Set (1, 0, 1);
		rotationConfiguration [3].Set (0, 0, 1);

		blockConfiguration = rotationConfiguration;
		warpToNextPiecePosition ();
	}

	public override Material getBlockMaterial(){
		return(blockMaterial);
	}

	public override Vector3[] getNextRotationConfiguration(){
		return(rotationConfiguration);
	}

	public override Vector3[] getPreviousRotationConfiguration(){
		return(rotationConfiguration);
	}

}
