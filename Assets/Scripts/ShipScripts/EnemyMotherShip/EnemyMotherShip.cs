using UnityEngine;
using System.Collections;

//TODO: inherit from Ship instead so we can get modular mothership building, cant be arsed to create wings etc atm
public class EnemyMotherShip : Destructables {

	public EnemyShip spawnprefab;
	public int maxSpawns;
	public float spawninterval;
	public Weapon wep1, wep2;
	public ShieldHolder sh;

	Collider2D primaryTarget;
	ArrayList threats;
	float spawnedt;
	Weapon weapon1, weapon2;
	Shield _shield;



	void Start () {
		maxHealth = health = 200;
		
		Transform w1 = transform.FindChild("weaponpos1");
		Transform w2 = transform.FindChild("weaponpos2");

		_shield = sh.BuildShield (38f, collider2D as PolygonCollider2D);
	
		weapon1 = Instantiate(wep1, w1.position, w1.rotation) as Weapon;
		weapon2 = Instantiate(wep2, w2.position, w2.rotation) as Weapon;

		weapon1.transform.parent = w1;
		weapon2.transform.parent = w2;

		weapon1.parent = gameObject;
		weapon2.parent = gameObject;
	}

	protected virtual void OnCollisionEnter2D(Collision2D col) {
		base.OnCollisionEnter2D (col);
	}

	public override void Damage (float d)
	{
		base.Damage (_shield.Damage(d));
	}
	override public void Die() {
		//Destroy (gameObject, 0f);
	}

	void FixedUpdate() {
	}

	void SpawnShip() {
		if(Time.time > spawnedt + spawninterval) {
			spawnedt = Time.time;
			
			Transform t = transform.FindChild("spawnpos");
			Instantiate(spawnprefab, t.position, t.rotation);
		}
	}


	void Seek() {

		//turn
		Vector2 targetPos = primaryTarget.transform.position;
		Vector2 forward = -transform.up.normalized;
		Vector2 left = transform.right.normalized;
		Vector2 dir = (targetPos - (Vector2)transform.position).normalized;
		
		float dist = (targetPos - (Vector2)transform.position).magnitude;
		float maxTurnForce = 1000000f; //<-- lol wtf?
		float fdot = Vector2.Dot (forward, dir);
		float ldot = Vector2.Dot (left, dir);
		float angle = Vector2.Angle (forward, dir) * Mathf.Deg2Rad;
		float turnForce = maxTurnForce * angle * Time.deltaTime;
		
		if (ldot < 0) {
			turnForce = -turnForce;
		}
		
		rigidbody2D.AddTorque (turnForce);


		//thrust
		float maxThrustForce = 100000.0f;
		float thrustForce = maxThrustForce * Mathf.Max(fdot,0) * Time.deltaTime;
		
		rigidbody2D.AddForce (forward * thrustForce);


		//eliminate obstacles
		ArrayList obstacles = new ArrayList ();
		Vector2 p1, p2;

		p1 = (Vector2)transform.position - forward * 200f - left * 20;
		p2 = (Vector2)transform.position + forward * 200f + left * 20;


		obstacles.AddRange (Physics2D.OverlapAreaAll (p1, p2, 1<<10));
		Collider2D closest = obstacles [0] as Collider2D;

		if (!closest)
			return;

		float highestdot = Vector2.Dot((closest.transform.position - transform.position).normalized, forward);
		foreach (Collider2D c in obstacles) {
			float dot = Vector2.Dot((c.transform.position - transform.position).normalized, forward);
			if(dot > highestdot) {
				closest = c;
				highestdot = dot;
			}
		}

		FireOnPos (closest.transform.position);

	}

	void FireOnPos(Vector2 targetPos) {
		weapon1.Rotate(targetPos);
		weapon2.Rotate(targetPos);


		float dot1 = Vector2.Dot (weapon1.transform.up, (targetPos - (Vector2)weapon1.transform.position).normalized);
		float dot2 = Vector2.Dot (weapon2.transform.up, (targetPos - (Vector2)weapon2.transform.position).normalized);

		if(dot1 > 0.8f)
			weapon1.Fire();

		if(dot2 > 0.8f)
			weapon2.Fire();
	}

	// Update is called once per frame
	void Update () {
	
		
		Collider2D t = Physics2D.OverlapCircle(transform.position, 200.0f, 1<<8);
		if (!primaryTarget)
			primaryTarget = t;

		if (!t && primaryTarget) { 
			Seek ();
		} else if(primaryTarget) {
			FireOnPos (primaryTarget.transform.position);
		}


	}
}
