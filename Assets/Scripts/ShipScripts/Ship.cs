using UnityEngine;
using System.Collections;

public abstract class Ship : Destructables {
	public Weapon[] weapons;
	public Engine[] engines;
	public Cockpit cockpit;
	public Hull hull;
	public Wings wings;
	public ParticleSystem deathEffect;

	Weapon[] activeWeapons;

	protected virtual void Start () {
		weapons = GetComponentsInChildren<Weapon> ();
		engines = GetComponentsInChildren<Engine> ();
		cockpit = GetComponentInChildren<Cockpit> ();
		hull = GetComponentInChildren<Hull> ();
		wings = GetComponentInChildren<Wings> ();

		maxHealth = health = hull.maxHealth;
		rigidbody2D.mass = hull.mass;

		activeWeapons = (Weapon[])weapons.Clone ();
	}

	protected virtual void FixedUpdate() {
		oldVelocity = rigidbody2D.velocity;
	}

	protected void ThrustEngines() {
		foreach (Engine e in engines)
			e.Thrust();
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
		foreach (Weapon w in activeWeapons)
			w.SendMessage("Rotate",target);
	}

	protected void FireWeapons() {
		foreach (Weapon w in activeWeapons)
			w.Fire ();
	}

	override public void Die() {
		deathEffect = Instantiate (deathEffect, transform.position, deathEffect.transform.rotation) as ParticleSystem;
		Destroy (gameObject, 0f);
		Destroy (deathEffect.gameObject, deathEffect.startLifetime);
	}
}
