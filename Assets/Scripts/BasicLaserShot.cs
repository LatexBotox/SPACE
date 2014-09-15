using UnityEngine;
using System.Collections;

public class BasicLaserShot : MonoBehaviour {

	float speed = 100f;
	float damage = 100;
	public ParticleSystem impact;

	Vector2 dir;

	void Start() {
		Destroy (gameObject, 10f);
		dir = new Vector2 (Mathf.Cos ((rigidbody2D.rotation+90) * Mathf.Deg2Rad), Mathf.Sin ((rigidbody2D.rotation+90) * Mathf.Deg2Rad)).normalized;
		rigidbody2D.velocity = dir.normalized*speed;
	}

	void OnCollisionEnter2D(Collision2D other) {
		ParticleSystem impactClone = (ParticleSystem)Instantiate (impact, other.contacts[other.contacts.Length-1].point,
		                                                          Quaternion.LookRotation(other.contacts[other.contacts.Length-1].normal));
		Destroy (impactClone.gameObject, impactClone.duration);
		Destroy (gameObject,0f);
		other.gameObject.SendMessage("Damage", damage); 
	}
}
