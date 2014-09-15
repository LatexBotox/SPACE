using UnityEngine;
using System.Collections;

public abstract class Destructables : MonoBehaviour
{
	protected double health = 1;
	protected double maxHealth = 1;

	public void Damage(double d) {
		health -= d;
		health = (double)Mathf.Max ((float)health, 0);
		if (health <= 0) {
			Die ();
		}
	}

	public void CalcColDamage(Collision2D col) {

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

