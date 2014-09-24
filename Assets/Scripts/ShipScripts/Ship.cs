using UnityEngine;
using System.Collections;

public abstract class Ship : Destructables {
	//public Weapon[] weapons;
	//public Engine[] engines;
	public Weapon weapon;
	public Engine engine;
	public Cockpit cockpit;
	public Hull hull;
	public Wings wings;
	public ParticleSystem deathEffect;
	public ParticleSystem colEffect;

	Light painLight;
	float hitTime;

	//Weapon[] activeWeapons;

	protected virtual void Start () {
		/*weapons = GetComponentsInChildren<Weapon> ();
		engines = GetComponentsInChildren<Engine> ();*/
		weapon = GetComponentInChildren<Weapon>();
		engine = GetComponentInChildren<Engine>();
		cockpit = GetComponentInChildren<Cockpit> ();
		hull = GetComponentInChildren<Hull> ();
		wings = GetComponentInChildren<Wings> ();

		maxHealth = health = hull.maxHealth;
		rigidbody2D.mass = hull.mass;

		//activeWeapons = weapons;
		hitTime = Time.time;
	}

	protected virtual void FixedUpdate() {
		oldVelocity = rigidbody2D.velocity;
	}

	protected void ThrustEngines() {
		/*foreach (Engine e in engines)
			e.Thrust();*/

		engine.Thrust();
	}

	protected void Dampen() {
		rigidbody2D.AddForce (rigidbody2D.velocity*-wings.dampeningFactor);
	}

	protected void RotateLeft() {
		rigidbody2D.AddTorque (wings.turnForce);
	}

	protected void RotateRight() {
		rigidbody2D.AddTorque (-wings.turnForce);
	}

	protected void RotateWeapons(Vector2 target) {
		/*foreach (Weapon w in weapons)
			w.SendMessage("Rotate",target);*/
		if(weapon != null) {
			//weapon.SendMessage("Rotate", target);
			weapon.Rotate(target);
			print ("rotate that weapon!");
		}
	}

	protected void FireWeapons() {
		/*foreach (Weapon w in weapons)
			w.Fire ();*/
		if(weapon != null) 
			weapon.Fire();
	}

	override public void Die() {
		deathEffect = Instantiate (deathEffect, transform.position, deathEffect.transform.rotation) as ParticleSystem;
		Destroy (gameObject, 0f);
		Destroy (deathEffect.gameObject, deathEffect.startLifetime);
	}

	protected override void OnCollisionEnter2D (Collision2D col)
	{
		base.OnCollisionEnter2D (col);
		if (!colEffect)
			return;
		GameObject impactClone = Instantiate(colEffect, transform.position, Quaternion.LookRotation(col.contacts[0].normal)) as GameObject;
		Destroy(impactClone);
	}
}
