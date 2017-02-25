using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*S-shaped piece*/
public class TetrisSBlock : TetrisBlock {
	private static Material blockMaterial = Resources.Load("Piece2_Mat", typeof(Material)) as Material;

	private Vector3[][] rotationConfigurations;

	public TetrisSBlock() : base(2) {
		blockModel [0].GetComponent<Renderer> ().material = blockMaterial;
		blockModel [1].GetComponent<Renderer> ().material = blockMaterial;
		blockModel [2].GetComponent<Renderer> ().material = blockMaterial;
		blockModel [3].GetComponent<Renderer> ().material = blockMaterial;

		maxRotationStates = 2;
		rotationConfigurations = new Vector3[2][];

		rotationConfigurations [0] = new Vector3[4];
		rotationConfigurations [0][0].Set (0, 0, 0);
		rotationConfigurations [0][1].Set (1, 0, 0);
		rotationConfigurations [0][2].Set (1, 0, 1);
		rotationConfigurations [0][3].Set (2, 0, 1);

		rotationConfigurations [1] = new Vector3[4];
		rotationConfigurations [1][0].Set (0, 0, 2);
		rotationConfigurations [1][1].Set (0, 0, 1);
		rotationConfigurations [1][2].Set (1, 0, 1);
		rotationConfigurations [1][3].Set (1, 0, 0);

		blockConfiguration = rotationConfigurations [0];
		warpToNextPiecePosition ();
	}

	public override Material getBlockMaterial(){
		return(blockMaterial);
	}

	public override Vector3[] getNextRotationConfiguration(){
		return(rotationConfigurations[(rotateState + 1) % maxRotationStates]);
	}

	public override Vector3[] getPreviousRotationConfiguration(){
		return(rotationConfigurations[System.Math.Abs(rotateState - 1) % maxRotationStates]);
	}
}
