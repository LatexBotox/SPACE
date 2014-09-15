using UnityEngine;
using System.Collections;


public abstract class Destructables : MonoBehaviour
{
	protected double health = 100;
	protected double maxHealth = 100;
	double col_dmg_scaler = 1;

	public double Col_dmg_scaler {
		get {
			return col_dmg_scaler;
		}
		set {
			col_dmg_scaler = value;
		}
	}

	public void Damage(double d) {
		print ("amam gawd dmg: " + d);
		health -= d;
		health = (double)Mathf.Max ((float)health, 0);
		if (health <= 0) {
			Die ();
		}
	}

	public double CalcColDamage(Collision2D col) { 
		double m = col.rigidbody.mass + rigidbody2D.mass; 
		double v = col.relativeVelocity.magnitude; 
		double e = 0.5*m*v*v;
		double dmg_frac = rigidbody2D.mass/m; 
		return e*dmg_frac*col_dmg_scaler;
	}

	public abstract void Die();

	public double Health {
		get {
			return health;
		}
	}

	public double MaxHealth {
		get {
			return maxHealth;
		}
	}

	public double Fraction() {
		return health/maxHealth;
	}
}

