using UnityEngine;
using System.Collections;

public class CannonScript : MonoBehaviour {

	public Weapon weap;
	public GameObject spawnpoint;

	void Start() {
		weap.Reset();
	}

	private void Rotate() {
		Vector2 relPos = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
		float a = Mathf.Atan2 (relPos.x*-1, relPos.y) * Mathf.Rad2Deg;
		Quaternion rotation = transform.rotation;
		rotation.eulerAngles = new Vector3(0,0,a);
		
		transform.rotation = rotation;
	}

	void Update () {

		Rotate();
		
		if (Input.GetKey(KeyCode.Mouse0)) {
			weap.Fire(spawnpoint);
		}
	}
}
