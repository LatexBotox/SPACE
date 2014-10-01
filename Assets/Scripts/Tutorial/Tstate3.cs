using UnityEngine;
using System.Collections;

public class Tstate3 : TutorialState
{
	public override void TriggerEnter(StateTrigger t, Collider2D c) {
		exit = true;		
	}

	public override void Run ()
	{
		tControl.DisplayMessage("Great Scott! I thought you were lost!" +
			"We were sucked into <insert plot device here> and ended up here. " +
			"You seem to have lost your mining laser, get into the hangar so we can requip your ship!", "Captain Stenis", 10.0f);
	}
}