using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPiece_Holder : Piece_Holder {

	// Use this for initialization
	void Start () {
		savedBlock = null;
		blockLocation = GetComponent<Transform> ().position + new Vector3(-1, 1, -1);
	}



}
