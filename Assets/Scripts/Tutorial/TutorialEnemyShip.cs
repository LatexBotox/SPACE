using UnityEngine;
using System.Collections;

public class TutorialEnemyShip : EnemyShip
{
	public Ship mtarget;

	void Start() {
		gameObject.SetActive(false);
	}

	protected override void FixedUpdate ()
	{
		moveTowards (pf.FindPath(this, mtarget));
		RotateWeapons (target.rigidbody2D.position + Random.insideUnitCircle * 7);
		FireWeapons();
	}
}