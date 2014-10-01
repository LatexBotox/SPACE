using UnityEngine;
using System.Collections;

public class Tstate7 : TutorialState
{
	public override void Run ()
	{
		tControl.DisplayMessage("Yay you saved the day! Now get back here so we can start working on getting home!", "Captain Stenis", 5.0f);
	}

	public override void TriggerEnter(StateTrigger t, Collider2D c)
	{
		tControl.TutorialDone();
	}
}

