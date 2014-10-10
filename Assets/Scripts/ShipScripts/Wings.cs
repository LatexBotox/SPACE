using UnityEngine;
using System.Collections;

public class Wings : ShipComponent
{
	public float dampeningFactor;
	public float turnForce;

	public float maxShield;
	public float curShield;

	public float shieldRegen;
	public float shieldCooldown;
	float regenStartsAt;

	public float spillMult;

	public float colSelfMult = 1;
	public float colVictimMult = 1;

	public Ship parent;

	public ShieldImpact impact;
	public SpriteRenderer shieldSprite;

	void Start() {
		Mathf.Clamp(curShield, 0, maxShield);
		regenStartsAt = 0;

		if (curShield == 0&&shieldSprite)
			shieldSprite.enabled = false;
	}

	void Update() {
		if(Time.time > regenStartsAt && curShield < maxShield) {
			parent.EnableShields();
			if (shieldSprite)
				shieldSprite.enabled = true;
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
			if (shieldSprite)
				shieldSprite.enabled = false;
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

	public void Impact(Vector2 impactDir) {
		Impact (impactDir, null);
	}

	public void Impact(Vector2 impactDir, Destructables target) {
		ShieldImpact clone = Instantiate (impact, parent.transform.position+new Vector3(0,0,-2), Quaternion.LookRotation(transform.forward, impactDir)) as ShieldImpact;
		clone.transform.parent = parent.transform;
	}
}

