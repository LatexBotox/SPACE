using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Asteroid : Destructables {
	public AsteroidGenerator gen;
	public ParticleSystem deathFX;
	
	public Chunk chunk;

	public MineralType mineral;


	public int sizeClass;
	public bool flagged;

	void Start () {
		
	}

	void Update () {
	
	}

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
			return new Color(0/255f,255/255f,0/255f);
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
				Asteroid clone = gen.GenerateAsteroid(mineral, sizeClass-1);
				clone.rigidbody2D.AddForce ((gen.transform.position-transform.position).normalized*1000*sizeClass);

				clone.chunk = chunk;

				if(chunk != null)
					chunk.AddAsteroid (clone);
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
		if (!flagged && chunk) {
			chunk.FlagAsteroid(this);
			flagged = true;
		}
	}
}
