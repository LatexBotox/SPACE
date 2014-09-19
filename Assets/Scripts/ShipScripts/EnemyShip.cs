using UnityEngine;
using System.Collections;

public class EnemyShip : Ship
{
	public PathFinder pf;
	protected Ship target;

	protected override void Start ()
	{
		base.Start ();
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
		target = Physics2D.OverlapCircle (rigidbody2D.position, cockpit.range, 1 << 8).GetComponent<Ship>();
		print (target.name);
	}
}

