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

	}

	void Dstroy() {
		weapon1.Rotate(primaryTarget.transform.position);
		weapon2.Rotate(primaryTarget.transform.position);
		weapon1.Fire();
		weapon2.Fire();
		
		SpawnShip();
	}

	// Update is called once per frame
	void Update () {
	
		
		Collider2D t = Physics2D.OverlapCircle(transform.position, 300.0f, 1<<8);
		if (!primaryTarget)
			primaryTarget = t;

		if (!t && primaryTarget) { 
			Seek ();
		} else if(primaryTarget) {
			Dstroy ();
		}


	}
}
