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
	public Transform weapPos;


	Collider2D shipCollider;
	Collider2D shieldCollider;

	Light painLight;
	float hitTime;

	//Weapon[] activeWeapons;

	protected virtual void Start () {
		/*weapons = GetComponentsInChildren<Weapon> ();
		engines = GetComponentsInChildren<Engine> ();*/
//		weapon = GetComponentInChildren<Weapon>();
//		engine = GetComponentInChildren<Engine>();
//		cockpit = GetComponentInChildren<Cockpit> ();
//		hull = GetComponentInChildren<Hull> ();
//		wings = GetComponentInChildren<Wings> ();
//
//		maxHealth = health = hull.maxHealth;
//		rigidbody2D.mass = hull.mass;
//
//		shipCollider = GetComponentInChildren<PolygonCollider2D>();
//		if (wings.maxShield > 0) {
//			wings.parent = this;
//			shipCollider.enabled = false;
//			shieldCollider = GetComponentInChildren<CircleCollider2D>();
//			shieldCollider.enabled = true;
//		}
//
//		//activeWeapons = weapons;
//		hitTime = Time.time;
	}

	public virtual void Init() {
		weapon = GetComponentInChildren<Weapon>();
		engine = GetComponentInChildren<Engine>();
		cockpit = GetComponentInChildren<Cockpit> ();
		hull = GetComponentInChildren<Hull> ();
		wings = GetComponentInChildren<Wings> ();
		
		maxHealth = health = hull.maxHealth;
		rigidbody2D.mass = hull.mass;
		
		shipCollider = GetComponentInChildren<PolygonCollider2D>();
		if (wings.maxShield > 0) {
			wings.parent = this;
			shipCollider.enabled = false;
			shieldCollider = GetComponentInChildren<CircleCollider2D>();
			shieldCollider.enabled = true;
		}
		
		//activeWeapons = weapons;
		hitTime = Time.time;
	}

	public void SetWeapon(Weapon weap) {
		if(weapon != null)
			Destroy(weapon.gameObject, 0.0f);

		weapon = Instantiate(weap) as Weapon;
		weapon.transform.parent = weapPos;
		weapon.transform.localPosition = Vector3.zero;
		weapon.transform.rotation = weapPos.rotation;
		weapon.gameObject.SetActive(true);
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
		rigidbody2D.AddForce (rigidbody2D.velocity*-wings.dampeningFactor*Time.deltaTime, ForceMode2D.Impulse);
	}

	protected void RotateLeft() {
		rigidbody2D.AddTorque (wings.GetTurnForce()*Time.deltaTime, ForceMode2D.Impulse);
	}

	protected void RotateRight() {
		rigidbody2D.AddTorque (-wings.GetTurnForce()*Time.deltaTime, ForceMode2D.Impulse);
	}

	protected void RotateWeapons(Vector2 target) {
		/*foreach (Weapon w in weapons)
			w.SendMessage("Rotate",target);*/
		if(weapon != null) {
			//weapon.SendMessage("Rotate", target);
			weapon.Rotate(target);
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
		if (wings && wings.curShield>0)
			wings.Impact (col.contacts[0].point-rigidbody2D.position);
		base.OnCollisionEnter2D (col);

		if (!colEffect)
			return;
		GameObject impactClone = Instantiate(colEffect, transform.position, Quaternion.LookRotation(col.contacts[0].normal)) as GameObject;
		Destroy(impactClone);
	}
	public override void Damage (float d)
	{
		if (wings.maxShield>0)
			d = wings.Damage(d);
		base.Damage (d);
	}

	public void EnableShields() {
		shipCollider.enabled = false;
		shieldCollider.enabled = true;
	}

	public void DisableShields() {
		shipCollider.enabled = true;
		shieldCollider.enabled = false;
	}
}
