using UnityEngine;
using System.Collections;

public class CustomInput : MonoBehaviour {
	KeyCode thrustKey;
	KeyCode dampenKey;
	KeyCode rotateLKey;
	KeyCode rotateRKey;
	KeyCode shootKey;

	public bool controlsEnabled = true;

	void Start () {
		thrustKey = KeyCode.W;
		dampenKey = KeyCode.LeftShift;
		rotateLKey = KeyCode.A;
		rotateRKey = KeyCode.D;
		shootKey = KeyCode.Mouse0;
	}
	
	public bool Thrust() {
		return Input.GetKey (thrustKey) && controlsEnabled;
	}

	public bool Dampen() {
		return Input.GetKey (dampenKey) && controlsEnabled;
	}

	public bool RotateL() {
		return Input.GetKey (rotateLKey) && !Input.GetKey (rotateRKey) && controlsEnabled;
	}

	public bool RotateR() {
		return Input.GetKey (rotateRKey) && !Input.GetKey (rotateLKey) && controlsEnabled;
	}

	public bool Shoot() {
		return Input.GetKey (shootKey) && controlsEnabled;
	}
}
