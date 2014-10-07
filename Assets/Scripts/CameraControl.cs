using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public GameObject follow;

	// Use this for initialization
	void Start () {
		if (!follow)
			follow = GameObject.FindWithTag ("Player");

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(!follow || !follow.activeSelf) {
			follow = (PlayerShip.instance?PlayerShip.instance.gameObject:null);
			return;
		}

		Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 shipPos = follow.transform.position;

		mouse.z = shipPos.z = transform.position.z;


		transform.position = (Vector3.Lerp (shipPos, mouse, 0.3f));


	}
}
