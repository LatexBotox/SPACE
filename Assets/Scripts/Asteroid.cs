using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Asteroid : Destructables {
	public AsteroidGenerator gen;

	public Mineral mineral;
	public enum Mineral {
		Iron,
		Copperium,
		Gallium,
		Whatium
	}

	public int sizeClass;

	void Start () {
		
	}

	void Update () {
	
	}

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
		if (sizeClass>0 && gen!=null) {
			for(int i = 0; i < 3;i++) {
				gen.transform.position = transform.position+(Vector3)Random.insideUnitCircle.normalized*5*sizeClass;
				Asteroid clone = gen.GenerateAsteroid(mineral, sizeClass-1);
				clone.rigidbody2D.AddForce ((gen.transform.position-transform.position).normalized*500*sizeClass);
			}
		}
		Destroy (gameObject, 0);
	}
}
