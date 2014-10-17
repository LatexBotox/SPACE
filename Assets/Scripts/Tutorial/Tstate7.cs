using UnityEngine;
using System.Collections;

public class Tstate7 : TutorialState
{
	public override void Run ()
	{
		Messenger.instance.AddMessage(new Message("Yay you saved the day!\nNow get back here so we can start working on getting home!", "Captn S"));

	}

	public override void EnterBox(StateTrigger t)
	{
		tControl.TutorialDone();
	}
}

