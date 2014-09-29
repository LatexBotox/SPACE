using UnityEngine;
using System.Collections;

public class Wings : MonoBehaviour
{
	public float dampeningFactor;
	public float turnForce;

	public float maxShield;
	public float curShield;

	public float shieldRegen;
	public float shieldCooldown;
	float regenStartsAt;

	public float spillMult;

	public Ship parent;

	void Start() {
		curShield = maxShield;

		regenStartsAt = 0;
	}

	void Update() {
		if(Time.time > regenStartsAt && curShield < maxShield) {
			parent.EnableShields();
			curShield = Mathf.Clamp(curShield+shieldRegen*Time.deltaTime, 0, maxShield);
		}
	}

	public float Damage(float d) {
		regenStartsAt = Time.time+shieldCooldown;

		float spill = d;
		if (curShield > 0) 
			spill = Mathf.Max (0,d-curShield)*spillMult;

			

		curShield = Mathf.Clamp (curShield-d, 0, maxShield);

		if (curShield == 0) {
			parent.DisableShields();
		}

		return spill;
	}

	public float Fraction() {
		if (maxShield>0) 
			return curShield/maxShield;
		return 0;
	}

	public float GetTurnForce() {
		if (curShield > 0)
			return turnForce*2f;

		return turnForce;
	}

}

