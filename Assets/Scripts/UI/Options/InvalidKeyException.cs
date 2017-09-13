using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvalidKeyException : System.Exception {
	public InvalidKeyException() : base(){}
	public InvalidKeyException(string message) : base(message) {}
	public InvalidKeyException(string message, System.Exception inner) : base(message, inner) {}
}
