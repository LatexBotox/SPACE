using UnityEngine;
using System.Collections;

public class ShipController : Destructables {
	Vector2 relPos;
	public float acceleration = 100f;

	public GameObject cannon;

	public Weapon[] weapons;
	public Engine[] engines;

	private Vector2 velocity;

	float thrust = 0;
	
	ParticleSystem particles;


	void Start () {
		health = maxHealth = 50;
		Col_dmg_scaler = 0.1f;
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


	void Update() {
		oldVelocity = rigidbody2D.velocity;
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

		if (Input.GetAxis ("Fire1") > 0)
			foreach (Weapon w in weapons)
				w.Fire ();

		velocity = rigidbody2D.velocity;
	}

	override public void Die() {
	}
}
