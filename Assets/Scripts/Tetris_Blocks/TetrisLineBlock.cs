using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Vertical line piece*/
public class TetrisLineBlock : TetrisBlock {
	private static Material blockMaterial = Resources.Load("Piece1_Mat", typeof(Material)) as Material;

	private Vector3[][] rotationConfigurations;

	public TetrisLineBlock() : base(1) {
		blockModel [0].GetComponent<Renderer> ().material = blockMaterial;
		blockModel [1].GetComponent<Renderer> ().material = blockMaterial;
		blockModel [2].GetComponent<Renderer> ().material = blockMaterial;
		blockModel [3].GetComponent<Renderer> ().material = blockMaterial;

		maxRotationStates = 2;
		rotationConfigurations = new Vector3[2][];

		rotationConfigurations [0] = new Vector3[4];
		rotationConfigurations [0][0].Set (1, 0, 0);
		rotationConfigurations [0][1].Set (1, 0, 1);
		rotationConfigurations [0][2].Set (1, 0, 2);
		rotationConfigurations [0][3].Set (1, 0, 3);

		rotationConfigurations [1] = new Vector3[4];
		rotationConfigurations [1][0].Set (0, 0, 2);
		rotationConfigurations [1][1].Set (1, 0, 2);
		rotationConfigurations [1][2].Set (2, 0, 2);
		rotationConfigurations [1][3].Set (3, 0, 2);

		blockConfiguration = rotationConfigurations [0];
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
