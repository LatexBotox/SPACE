using UnityEngine;
using System.Collections;

public class ShipController : Destructables {
	Vector2 relPos;
	public float acceleration = 100f;

	public GameObject cannon;

	public Engine[] engines;

	private Vector2 velocity;

	float thrust = 0;
	
	ParticleSystem particles;


	void Start () {
		health = maxHealth = 50;
		particles = GetComponentInChildren<ParticleSystem>();
	}

	void StartEngines() {
		foreach (Engine e in engines)
			e.Begin ();
	}
	
	void StopEngines() {
		foreach (Engine e in engines)
			e.End ();
	}


	void FixedUpdate () {

		if (thrust != Input.GetAxis ("Thrust")) {
			thrust = Input.GetAxis ("Thrust");
			if(thrust>0) {
				StartEngines ();
			} else {
				StopEngines ();
			}
		}

		Camera.main.orthographicSize = Mathf.Lerp (45, 55, rigidbody2D.velocity.magnitude/150);



		if (Input.GetAxis ("Rotation")<0) {
			rigidbody2D.AddTorque (acceleration);
		} else if (Input.GetAxis ("Rotation")>0) {
			rigidbody2D.AddTorque(-acceleration);
		}

		if (thrust < 0) 
			rigidbody2D.AddForce (rigidbody2D.velocity*-6);


		if (thrust > 0) 
			foreach (Engine e in engines)
				e.Thrust();

		velocity = rigidbody2D.velocity;
	}


	void OnCollisionEnter2D(Collision2D col) {
		//double d = (rigidbody2D.velocity-velocity).sqrMagnitude*0.01;
		double d = CalcColDamage(col);
		Debug.Log (d);
		Damage (d);
	}
}
