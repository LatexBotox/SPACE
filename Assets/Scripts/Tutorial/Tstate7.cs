using UnityEngine;
using System.Collections;

public class Tstate7 : TutorialState
{
	public override void Run ()
	{
		tControl.DisplayMessage("Yay you saved the day! Now get yo fat ass in here so we can git the hell aouta this stinking shithole", "Captain Stenis", 5.0f);
	}

	public override void TriggerEnter(StateTrigger t, Collider2D c)
	{
		tControl.TutorialDone();
	}
}

