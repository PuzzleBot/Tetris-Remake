using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*L-shaped piece*/
public class TetrisLBlock : TetrisBlock {
	private static Material blockMaterial = Resources.Load("Piece5_Mat", typeof(Material)) as Material;

	private Vector3[][] rotationConfigurations;

	public TetrisLBlock() : base(5) {
		blockModel [0].GetComponent<Renderer> ().material = blockMaterial;
		blockModel [1].GetComponent<Renderer> ().material = blockMaterial;
		blockModel [2].GetComponent<Renderer> ().material = blockMaterial;
		blockModel [3].GetComponent<Renderer> ().material = blockMaterial;

		maxRotationStates = 4;
		rotationConfigurations = new Vector3[4][];

		rotationConfigurations [0] = new Vector3[4];
		rotationConfigurations [0][0].Set (1, 0, 0);
		rotationConfigurations [0][1].Set (2, 0, 0);
		rotationConfigurations [0][2].Set (1, 0, 1);
		rotationConfigurations [0][3].Set (1, 0, 2);

		rotationConfigurations [1] = new Vector3[4];
		rotationConfigurations [1][0].Set (0, 0, 0);
		rotationConfigurations [1][1].Set (0, 0, 1);
		rotationConfigurations [1][2].Set (1, 0, 1);
		rotationConfigurations [1][3].Set (2, 0, 1);

		rotationConfigurations [2] = new Vector3[4];
		rotationConfigurations [2][0].Set (0, 0, 2);
		rotationConfigurations [2][1].Set (1, 0, 2);
		rotationConfigurations [2][2].Set (1, 0, 1);
		rotationConfigurations [2][3].Set (1, 0, 0);

		rotationConfigurations [3] = new Vector3[4];
		rotationConfigurations [3][0].Set (2, 0, 2);
		rotationConfigurations [3][1].Set (2, 0, 1);
		rotationConfigurations [3][2].Set (1, 0, 1);
		rotationConfigurations [3][3].Set (0, 0, 1);

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
