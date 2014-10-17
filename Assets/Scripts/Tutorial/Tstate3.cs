using UnityEngine;
using System.Collections;

public class Tstate3 : TutorialState
{

	public TutorialMothership mshipprefab;
	TutorialMothership mship;
	Animator hatch;

	Message m;


	public override void Run ()
	{

		Random.seed = (int)Time.time;
		Vector2 dir = Random.insideUnitCircle;
		Vector2 pos = (Vector2)PlayerShip.instance.transform.position + dir * 300f;
		Quaternion rot = Quaternion.identity;

		 
		mship = Instantiate (mshipprefab, pos, rot) as TutorialMothership;
		hatch = mship.GetComponent<Animator> ();

		mship.SetCircleEnabled (true);
		mship.SetBoxEnabled (false);

		m = new Message ("Scanning,.. Fleet Signal confirmed; Waypoint established.", "Rob");
		Messenger.instance.AddMessage (m);
	}

	public override void EnterCircle(StateTrigger t) {
		t.SetEnabled (false);
		Messenger.instance.KillMessage (m);
		m = new Message ("Great Scott! I thought you were lost!\n" +
			"We were sucked into <insert plot device here>\nand ended up here.\n", "Captn S");
		Messenger.instance.AddMessage (m);
		m = new Message("You seem to have lost your mining laser,\nget into the hangar so we can get you a new one!", "Captn S" , callback: OpenHatch);
		Messenger.instance.AddMessage (m);


	}

	public void OpenHatch() {
		hatch.SetBool("Open", true);
		mship.SetBoxEnabled (true);
	}

	public override void EnterBox(StateTrigger t) {
		Messenger.instance.KillMessage (m);
		exit = true;
	}
}