using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Asteroid : Destructables {
	public AsteroidGenerator gen;
	public ParticleSystem deathFX;
	
	public Chunk chunk;
	public int id;

	public MineralType mineral;

	public int sizeClass;
	public bool flagged;

	Vector2 lastColPoint;

	public static Color MineralToColor(MineralType mineral) {
		switch(mineral) {
		case(MineralType.Iron):
			return new Color(225/255f, 179/255f, 121/255f);
		case(MineralType.Copperium):
			return new Color(215/255f, 225/255f, 121/255f);
		case(MineralType.Gallium):
			return new Color(210/255f, 255/255f, 255/255f);
		case(MineralType.Whatium):
			return new Color(255/255f, 141/255f, 247/255f);
		default:
			return Color.white;
		}
	}

	public override void Die ()
	{
		Destroy (gameObject, 0);

		if(chunk != null) {
			if (!flagged) {
				chunk.FlagAsteroid(this);
				flagged = true;
			}
			
			chunk.RemoveAsteroid (this);
		}

		if (sizeClass>0 && gen!=null) {
			for(int i = 0; i < 3;i++) {
				gen.transform.position = transform.position+(Vector3)Random.insideUnitCircle.normalized*5*sizeClass;
				Asteroid clone = gen.GenerateAsteroid(mineral, sizeClass-1,chunk.chunkSeed+Random.Range(int.MinValue,int.MaxValue)+id);

				clone.chunk = chunk;

				if(chunk != null)
					chunk.AddAsteroid (clone);
				else
					clone.gameObject.SetActive(true);
					
				clone.rigidbody2D.AddForce (((Vector2)gen.transform.position-lastColPoint).normalized*10*sizeClass, ForceMode2D.Impulse);
			}
		} //else if(sizeClass == 0) {

		if (mineral != MineralType.Blank) {
			Mineral drop = gen.GenerateMineral(mineral);
			drop.transform.position = transform.position;
		}
//		}

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
