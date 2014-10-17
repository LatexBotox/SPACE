using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public GameObject follow;

	float minZoom = 30;
	float maxZoom = 80;
	float currentZoom = 45;

	// Use this for initialization
	void Start () {


	}


	
	// Update is called once per frame
	void FixedUpdate () {
		if(!follow || !follow.activeSelf) {
			follow = (PlayerShip.instance?PlayerShip.instance.gameObject:null);
			return;
		}

		camera.orthographicSize = Mathf.Lerp (currentZoom, currentZoom+20f, follow.rigidbody2D.velocity.magnitude/100);

		Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 shipPos = follow.transform.position;

		mouse.z = shipPos.z = transform.position.z;


		transform.position = (Vector3.Lerp (shipPos, mouse, 0.3f));


	}
}
