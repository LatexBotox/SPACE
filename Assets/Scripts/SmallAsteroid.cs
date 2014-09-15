using UnityEngine;
using System.Collections;

public class SmallAsteroid : Destructables
{

		// Use this for initialization
	void Start ()	
	{
		Col_dmg_scaler = 0.5;
		health = 100;
		maxHealth = 100;
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	public override void Die()
	{
		Destroy(gameObject);
	}

	void OnCollisionEnter2D(Collision2D col) {
		double d = CalcColDamage(col);
		print ("dmg: " + d);
		Damage(d);
	}
}

