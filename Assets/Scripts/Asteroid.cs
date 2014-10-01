using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Asteroid : Destructables {
	public AsteroidGenerator gen;
	public ParticleSystem[] deathFX;
	
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
			if (chunk != null)
				Random.seed = chunk.chunkSeed+id;
			float numAsteroids = Random.Range(sizeClass, sizeClass+3);

			float angleStep = 360/numAsteroids;
			float startAngle = Random.Range (0,360);

			for(int i = 0; i < numAsteroids;i++) {
				gen.transform.position = transform.position + (Vector3)new Vector2(Mathf.Cos (Mathf.Deg2Rad*(startAngle+angleStep*i)), 
				                                                          				 Mathf.Sin (Mathf.Deg2Rad*(startAngle+angleStep*i)))*(sizeClass+1)*2;
				Asteroid clone = gen.GenerateAsteroid(mineral, sizeClass-1,chunk.chunkSeed+Random.Range(int.MinValue,int.MaxValue)+id);

				clone.chunk = chunk;

				if(chunk != null)
					chunk.AddAsteroid (clone);
				else
					clone.gameObject.SetActive(true);
					
				clone.rigidbody2D.AddForce (((Vector2)gen.transform.position-lastColPoint).normalized*10*sizeClass, ForceMode2D.Impulse);
			}
		}

		if (mineral != MineralType.Blank) {
			Mineral drop = gen.GenerateMineral(mineral);
			drop.transform.position = transform.position;
		}

		if(deathFX.Length>0) {
			Instantiate(deathFX[(int)Mathf.Clamp(sizeClass,0,deathFX.Length-1)], transform.position+new Vector3(0,0,-5), transform.rotation);
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
