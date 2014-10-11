using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour
{

	public float maxShield;
	public float curShield;
	
	public float shieldRegen;
	public float shieldCooldown;
	public float spillMult;
	public float radius;
	public CircleCollider2D shcollider;
	public PolygonCollider2D pcollider;

	float regenStartsAt;

	public ShieldImpact impact;
	public SpriteRenderer shieldSpritePrefab;


	SpriteRenderer _shieldSprite;

	// Use this for initialization
	void Start ()
	{

		_shieldSprite = Instantiate (shieldSpritePrefab, transform.position, transform.rotation) as SpriteRenderer;
		_shieldSprite.transform.parent = transform;
		_shieldSprite.transform.localScale = new Vector3 (radius*0.78f, radius*0.78f, 1);
		shcollider.radius = radius;

		_shieldSprite.enabled = curShield > 0;
		shcollider.enabled = curShield > 0;
		pcollider.enabled = !shcollider.enabled;
	}
	
	// Update is called once per frame
	void Update ()
	{

		if(Time.time > regenStartsAt && curShield < maxShield) {
			pcollider.enabled = false;
			shcollider.enabled = true;
			_shieldSprite.enabled = true;
			curShield = Mathf.Clamp(curShield+shieldRegen*Time.deltaTime, 0, maxShield);
		}
	}

	public float Fraction() {
		if (maxShield>0) 
			return curShield/maxShield;
		return 0;
	}

	public float Damage(float d) {
		regenStartsAt = Time.time+shieldCooldown;
		
		float spill = d;
		if (curShield > 0)  
			spill = Mathf.Max (0,d-curShield)*spillMult;
		
		curShield = Mathf.Clamp (curShield-d, 0, maxShield);
		
		if (curShield <= 0) {
			pcollider.enabled = true;
			shcollider.enabled = false;
			_shieldSprite.enabled = false;
		}
		
		return spill;
	}
	
	public void Impact(Vector2 impactDir) {
		Vector3 pos = transform.position + (Vector3)impactDir.normalized * radius*0.92f + new Vector3 (0, 0, -2);
		ShieldImpact clone = Instantiate (impact, pos, Quaternion.LookRotation(transform.forward, impactDir)) as ShieldImpact;
		Debug.DrawLine (transform.position, pos);
	}
}

