using UnityEngine;
using System.Collections;

public class MediumAsteroid : Destructables
{
	public GameObject smallAsteroid;

	// Use this for initialization
	void Start ()	
	{
		Col_dmg_scaler = 0.5;
		health = 1000;
		maxHealth = 1000;
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	private void SpawnSmallAsteroid() {
		GameObject clone = Instantiate(smallAsteroid, transform.position, transform.rotation) as GameObject;
		Vector2 vel = rigidbody2D.velocity;
		Vector2 n = new Vector2(vel.y, -vel.x).normalized;
		clone.rigidbody2D.velocity = vel + n * 0.5f * vel.magnitude;
	}

	public override void Die()
	{
		//dead = true;
		SpawnSmallAsteroid();
		SpawnSmallAsteroid();
		Destroy(gameObject);
	}

	void OnCollisionEnter2D(Collision2D col) {
		double d = CalcColDamage(col);
		print ("dmg: " + d);
		Damage(d);
	}
}

