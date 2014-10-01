using UnityEngine;
using System.Collections;

public class EnemyShip : Ship
{
	public PathFinder pf;
	protected Ship target;

	protected override void Start ()
	{
		base.Start ();
		base.Init ();
		enabled = pf;
	}

	protected override void FixedUpdate ()
	{
		base.FixedUpdate ();
		if (!target) {
			FindTarget ();
			return;
		}

		moveTowards (pf.FindPath(this, target));
		if (CanSeeTarget()) {
			RotateWeapons (target.rigidbody2D.position + Random.insideUnitCircle * 7);
			FireWeapons();
		}
	}

	public void moveTowards(Vector2 pos) {
		Vector2 relpos = pos - rigidbody2D.position - rigidbody2D.velocity;
		float angle = Mathf.DeltaAngle(rigidbody2D.rotation, Mathf.Atan2 (relpos.y, relpos.x)*Mathf.Rad2Deg-90);
		
		if (Mathf.Abs (angle) < 90) 
			ThrustEngines ();
		
		if (angle > 5) {
			RotateLeft();
		} else if (angle < 5) {
			RotateRight ();
		}
		
		if (Mathf.Abs (angle) > 180) 
			Dampen ();
	}

	void FindTarget() {
		Collider2D potentialTarget = Physics2D.OverlapCircle (rigidbody2D.position, cockpit.range, 1 << 8);
		if (potentialTarget) {
			target = potentialTarget.GetComponent<Ship>();
		}
	}

	bool CanSeeTarget() {
		RaycastHit2D ray = Physics2D.Raycast (rigidbody2D.position, target.rigidbody2D.position - rigidbody2D.position, pf.preferredDistance*1.5f, 1 << 8 | 1 << 10);
		return ray.collider && ray.transform.gameObject == target.gameObject;
	}

	void DESTRUCTION() {
		Destroy (gameObject, 0f);
	}

	public void DespawnIn(float t) {
		CancelInvoke("DESTRUCTION");
		Invoke ("DESTRUCTION",t);
	}
}


