using UnityEngine;
using System.Collections;

public class BasicLaserShot : MonoBehaviour {

	public float speed = 100f;

	Vector2 dir;

	void Start() {
		Destroy (gameObject, 10f);
		dir = new Vector2 (Mathf.Cos ((rigidbody2D.rotation+90) * Mathf.Deg2Rad), Mathf.Sin ((rigidbody2D.rotation+90) * Mathf.Deg2Rad)).normalized;
		rigidbody2D.AddForce (dir * speed);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.layer == 9 || other.gameObject.layer == 10) {
			other.rigidbody2D.AddForce (dir*500);
			Destroy (gameObject,0f);
		}
	}
}
