using UnityEngine;
using System.Collections;

public class Tstate6 : TutorialState
{
	public EnemyShip enemy;
	public Transform spawnpos;

	EnemyShip clone;

	public override void Run ()
	{
		clone = Instantiate(enemy, spawnpos.position, spawnpos.transform.rotation) as EnemyShip;
		tControl.DisplayMessage("Ship detected! Closing in fast! Their signature reads.. Oh no.. SPACEPIRATES! Save me mommy!" ,"Captain Stenis", 10.0f);
	}
	
	public override void sUpdate ()
	{
		if(clone == null) {
			exit = true;
		}
	}
}

