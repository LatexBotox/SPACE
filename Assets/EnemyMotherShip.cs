using UnityEngine;
using System.Collections;

public class EnemyMotherShip : Ship {

	public EnemyShip spawnprefab;
	public int maxSpawns;
	public float spawninterval;
	public Weapon wep1, wep2;
	
	Collider2D target;
	float spawnedt;
	Weapon weapon1, weapon2;

	// Use this for initialization
	void Start () {
		Transform w1 = transform.FindChild("weaponpos1");
		Transform w2 = transform.FindChild("weaponpos2");

		weapon1 = Instantiate(wep1, w1.position, w1.rotation) as Weapon;
		weapon2 = Instantiate(wep2, w2.position, w2.rotation) as Weapon;
		weapon1.parent = gameObject;
		weapon2.parent = gameObject;
	}

	void FixedUpdate() {

	}

	// Update is called once per frame
	void Update () {

		target = Physics2D.OverlapCircle(transform.position, 300.0f, 1<<8);

		if(target) {
			weapon1.Rotate(target.transform.position);
			weapon2.Rotate(target.transform.position);
			weapon1.Fire();
			weapon2.Fire();

			if(Time.time > spawnedt + spawninterval) {
				spawnedt = Time.time;

				Transform t = transform.FindChild("spawnpos");
				Instantiate(spawnprefab, t.position, t.rotation);
			}

		}
	}
}
