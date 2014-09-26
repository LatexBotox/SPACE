using UnityEngine;
using System.Collections;

public class Tstate6 : TutorialState
{
	public GameObject mship;
	public TutorialEnemyShip enemy;

	private TutorialEnemyShip clone;

	public override void Run ()
	{
		tControl.DisplayMessage("OMG we are under attack!" ,"Captain Stenis", 10.0f);
		clone = Instantiate(enemy, mship.transform.position + mship.transform.up*100, Quaternion.identity) as TutorialEnemyShip;
		clone.mtarget = mship;
	}
	
	public override void sUpdate ()
	{
		if(clone == null) {
			exit = true;
		}
	}
}

