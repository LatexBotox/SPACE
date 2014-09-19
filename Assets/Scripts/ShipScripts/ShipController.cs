using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipController : Destructables {
	public float acceleration = 100f;

	public Weapon[] weapons;
	public Engine[] engines;

	public CustomInput input;
	public ParticleSystem deathEffect;

	bool thrust = false;

	void Start () {
		if (!input)
			enabled = false;
		health = maxHealth = 50;
		Col_dmg_scaler = 0.1f;

		weapons = GetComponentsInChildren<Weapon> ();
		engines = GetComponentsInChildren<Engine> ();
	}

//	void StartEngines() {
//		foreach (Engine e in engines)
//			e.Begin ();
//	}
//	
//	void StopEngines() {
//		foreach (Engine e in engines)
//			e.End ();
//	}

	void Update() {
		oldVelocity = rigidbody2D.velocity;
	}

	void FixedUpdate () {

		if (thrust != input.Thrust ()) {
			thrust = input.Thrust ();
			if(input.Thrust ()) {
//				StartEngines ();
			} else {
//				StopEngines ();
			}
		}

		Camera.main.orthographicSize = Mathf.Lerp (45, 55, rigidbody2D.velocity.magnitude/150);

		if (input.RotateL()) {
			rigidbody2D.AddTorque (acceleration);
		} else if (input.RotateR()) {
			rigidbody2D.AddTorque(-acceleration);
		}

		if (input.Dampen ()) 
			rigidbody2D.AddForce (rigidbody2D.velocity*-6);


		if (input.Thrust ()) 
			foreach (Engine e in engines)
				e.Thrust();

		if (input.Shoot ())
			foreach (Weapon w in weapons)
				w.Fire ();

		oldVelocity = rigidbody2D.velocity;
	}

	override public void Die() {
		GameObject deathEffectClone = Instantiate (deathEffect, transform.position, deathEffect.transform.rotation) as GameObject;
		Destroy (gameObject, 0f);
		Destroy (deathEffectClone, deathEffect.startLifetime);
	}

	public void moveTowards(Vector2 pos) {
		Vector2 relpos = pos - rigidbody2D.position - rigidbody2D.velocity;
		float angle = Mathf.DeltaAngle(rigidbody2D.rotation, Mathf.Atan2 (relpos.y, relpos.x)*Mathf.Rad2Deg-90);

		if (Mathf.Abs (angle) < 90) 
			foreach (Engine e in engines)
				e.Thrust();

		if (angle > 5) {
			rigidbody2D.AddTorque (acceleration*1f);
		} else if (angle < 5) {
			rigidbody2D.AddTorque (-acceleration*1f);
		}

		if (Mathf.Abs (angle) > 180) 
			rigidbody2D.AddForce (rigidbody2D.velocity*-6);
	}
}
