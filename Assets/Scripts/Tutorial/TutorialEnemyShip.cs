using UnityEngine;
using System.Collections;

public class TutorialEnemyShip : EnemyShip
{
	public GameObject mtarget;

	protected override void FixedUpdate ()
	{
		base.FixedUpdate ();
		
		moveTowards (pf.FindPath(this, target));
		RotateWeapons (target.rigidbody2D.position + Random.insideUnitCircle * 7);
		FireWeapons();
	}
}