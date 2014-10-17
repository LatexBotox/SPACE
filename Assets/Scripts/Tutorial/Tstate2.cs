using UnityEngine;
using System.Collections;

public class Tstate2 : TutorialState
{
	public PlayerShip pship;

	Message m;
	KeyListener kl;

	public override void Run ()
	{
		//tControl.DisplayMessage("Location unknown.. Attempting connection to main cluster.. ERROR 404\n" +
		//                        "Scanning,.. Fleet Signal confirmed; Waypoint established.",  "Rob", 10.0f);
		m = new Message ("Commence test of inertial dampeners;\nActivate at high velocity.\n(default key: LSHIFT)", "Rob");
		Messenger.instance.AddMessage (m);

		kl = new KeyListener (CustomInput.instance.DampenKey, DampenPressed);
		CustomInput.instance.RegDownListener(kl);
	}

	/*public override void TriggerEnter(StateTrigger t, Collider2D c)
	{
		t.gameObject.SetActive(false);
		exit = true;
	}*/

	public void DampenPressed() {
		float vel = PlayerShip.instance.rigidbody2D.velocity.magnitude;
		if (vel > 20.0f) {
			Messenger.instance.KillMessage(m);
			CustomInput.instance.RemDownListener(kl);

			m = new Message ("Inertial dampeners test complete.", "Rob", duration: 3f, callback: Done);
			Messenger.instance.AddMessage (m);
		}
	}

	public void Done() {
		exit = true;
	}

}

