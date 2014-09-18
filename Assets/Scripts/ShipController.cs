using UnityEngine;
using System.Collections;

public class ShipController : Destructables {
	Vector2 relPos;
	public float acceleration = 100f;

	public Weapon[] weapons;
	public Engine[] engines;

	public CustomInput input;

	bool thrust = false;

	void Start () {
		health = maxHealth = 50;
		Col_dmg_scaler = 0.1f;
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

		if (thrust != input.Thrust ()) {
			thrust = input.Thrust ();
			if(input.Thrust ()) {
				StartEngines ();
			} else {
				StopEngines ();
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
	}
}
