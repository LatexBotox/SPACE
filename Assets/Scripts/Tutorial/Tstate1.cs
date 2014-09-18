using UnityEngine;
using System.Collections;

public class Tstate1 : TutorialState
{

	private float tPlayed;
	private float hInterval = 20.0f;
	private float eTime;
	private bool exit = false;

	public Tstate1(ShipController _ship, TutorialController _tcontrol) : base(_ship, _tcontrol) {}

	public override void Start ()
	{
		tPlayed = Time.time;
		tcontrol.DisplayMessage("System recovery; \nRunning diagnostic.. \nCommence main engine test. (Default key: W)", "Rob1", 10.0f);
	}

	public override void Update ()
	{
		if(!exit && ship.rigidbody2D.velocity.magnitude > 10.0f) {
			tcontrol.DisplayMessage("Success; Main engine test complete.", "Rob2", 6.0f);
			exit = true;
			return;
		}

		if(!exit && Time.time > tPlayed + hInterval)
			Start ();

	}

	public override void Exit ()
	{

		tcontrol.NextState();
	}

	public override void GuiCallback ()
	{

		tcontrol.HideMessage();
		if(exit)
			Exit ();
	}
}

