using UnityEngine;
using System.Collections;

public class Tstate4 : TutorialState
{
	public CameraHandler camHandle;

	Message m;

	public override void Run ()
	{
		camHandle.ActivateHangarCam();
		m = new Message ("Nice landing! Now use the\nupgrade interface to get yourself\na shiny new laser!", "Captn S");
		Messenger.instance.AddMessage (m);
	}

	public override void sUpdate ()
	{
		if(PlayerShip.instance.weapon!=null) {
			PlayerShip.instance.weapon.enabled = false;
			Messenger.instance.KillMessage(m);
			m = new Message ("Sensors have detected a nearby asteroid\nIt might contain something useful.\nGo check it out pretty please.", "Captn S");
			Messenger.instance.AddMessage (m);
			exit = true;
		}
	}

	public override void EnterBox(StateTrigger t) {
		Messenger.instance.KillMessage (m);
		Run ();
	}
}

