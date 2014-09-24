using UnityEngine;
using System.Collections;


public abstract class Destructables : MonoBehaviour
{
	protected float health = 100;
	protected float maxHealth = 100;
	float col_dmg_scaler = 0.1f;

	protected Vector2 oldVelocity = new Vector2 (0, 0);

	public float Col_dmg_scaler {
		get {
			return col_dmg_scaler;
		}
		set {
			col_dmg_scaler = value;
		}
	}

	public virtual void Damage(float d) {
		health -= d;
		health = (float)Mathf.Max ((float)health, 0);
		if (health <= 0) {
			Die ();
		}
	}

	protected virtual void OnCollisionEnter2D(Collision2D col) {
		float deltaV = 0.5f*(oldVelocity-rigidbody2D.velocity).sqrMagnitude;
		float m = col.rigidbody.mass + rigidbody2D.mass; 
		
		Damage (deltaV*(col.rigidbody.mass/m)*col_dmg_scaler);
		col.gameObject.SendMessage ("Damage", (deltaV * (rigidbody2D.mass / m))*col_dmg_scaler);
	}

	public abstract void Die();

	public float Health {
		get {
			return health;
		}
		set {
			health = value;
		}
	}

	public float MaxHealth {
		get {
			return maxHealth;
		}
	}

	public float Fraction() {
		return health/maxHealth;
	}
}

