using UnityEngine;
using System.Collections;

public class EnemyShip : Ship
{
	public PathFinder pf;
	public Ship target;

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
		Vector2 mpos = transform.position;
		Vector2 forward = transform.up;
		Vector2 right = transform.right;
		Vector2 dir = (pos - mpos).normalized;
		float angle = Vector2.Angle (dir, forward) * Mathf.Deg2Rad;
		float rdot = Vector2.Dot (right, dir);


		if (rdot > 0.0f)
			RotateRight ();//RotateRight (angle);
		else
			RotateLeft(); //RotateLeft (angle);


		if (angle < 0.4)
			ThrustEngines ();



		/*Vector2 relpos = pos - rigidbody2D.position - rigidbody2D.velocity;
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

*/
	}

	void FindTarget() {
		Collider2D potentialTarget = Physics2D.OverlapCircle (rigidbody2D.position, cockpit.range, 1 << 8);
		if (potentialTarget) {
			target = potentialTarget.GetComponent<PlayerShip>();
		}
	}

	bool CanSeeTarget() {
		RaycastHit2D ray = Physics2D.Raycast (rigidbody2D.position, target.rigidbody2D.position - rigidbody2D.position, pf.preferredDistance*1.5f, 1 << 8 | 1 << 10);
		return ray.collider && ray.transform.gameObject == target.gameObject;
	}

	void DESTRUCTION() {
		Destroy (gameObject, 0f);
	}

	public override void CollisionDamage (float d, int layer)
	{	
		if (layer == gameObject.layer)
			base.CollisionDamage(d*0.1f,layer);
	}

	public void DespawnIn(float t) {
		CancelInvoke("DESTRUCTION");
		Invoke ("DESTRUCTION",t);
	}
}


