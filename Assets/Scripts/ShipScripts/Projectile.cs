using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	public float speed = 100;
	public float damage = 100;
	public ParticleSystem impact;

	public int launchedLayer = 0;


	Vector2 dir;
	
	void Start() {
		Destroy (gameObject, 10f);
		rigidbody2D.velocity = transform.up*speed;
	}
	
	void OnCollisionEnter2D(Collision2D other) {

		//print (other.gameObject.name);
		ParticleSystem impactClone = (ParticleSystem)Instantiate (impact, other.contacts[other.contacts.Length-1].point,
		                                                          Quaternion.LookRotation(other.contacts[other.contacts.Length-1].normal));
		Destroy (impactClone.gameObject, impactClone.duration);
		Destroy (gameObject,0f);

		if(other.gameObject.layer != launchedLayer)
			other.gameObject.SendMessageUpwards("Damage", damage); 
	}
}
