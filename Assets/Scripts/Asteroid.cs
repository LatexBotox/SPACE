using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Asteroid : Destructables {
	public AsteroidGenerator gen;
	public ParticleSystem deathFX;
	
	public Chunk chunk;
	public int id;

	public Mineral mineral;
	public enum Mineral {
		Iron,
		Copperium,
		Gallium,
		Whatium,
		Blank
	}

	public int sizeClass;
	public bool flagged;

	Vector2 lastColPoint;

	public static Color MineralToColor(Mineral mineral) {
		switch(mineral) {
		case(Mineral.Iron):
			return new Color(225/255f, 179/255f, 121/255f);
		case(Mineral.Copperium):
			return new Color(215/255f, 225/255f, 121/255f);
		case(Mineral.Gallium):
			return new Color(210/255f, 255/255f, 255/255f);
		case(Mineral.Whatium):
			return new Color(255/255f, 141/255f, 247/255f);
		default:
			return new Color(0/255f,255/255f,0/255f);
		}
	}

	public override void Die ()
	{
		Destroy (gameObject, 0);
		if (!flagged) {
			chunk.FlagAsteroid(this);
			flagged = true;
		}

		chunk.RemoveAsteroid (this);

		if (sizeClass>0 && gen!=null) {
			for(int i = 0; i < 3;i++) {
				gen.transform.position = transform.position+(Vector3)Random.insideUnitCircle.normalized*5*sizeClass;
				Asteroid clone = gen.GenerateAsteroid(mineral, sizeClass-1);

				chunk.AddAsteroid (clone);
				clone.chunk = chunk;

				clone.rigidbody2D.AddForce (((Vector2)gen.transform.position-lastColPoint).normalized*1000*sizeClass);
			}
		}

		if(deathFX) {
			deathFX = Instantiate(deathFX, transform.position, transform.rotation) as ParticleSystem;
			deathFX.transform.localScale = Vector3.one*sizeClass/2;
		}
	}

	protected override void OnCollisionEnter2D (Collision2D col)
	{
		base.OnCollisionEnter2D (col);
		lastColPoint = col.contacts[0].point+col.contacts[0].normal*5;
		if (!flagged && chunk) {
			chunk.FlagAsteroid(this);
			flagged = true;
		}
	}
}
