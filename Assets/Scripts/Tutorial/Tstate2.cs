using UnityEngine;
using System.Collections;

public class Tstate2 : TutorialState
{

	private float tPlayed;
	private float hInterval = 20.0f;

	public Tstate2(ShipController _ship, TutorialController _tcontrol) : base(_ship, _tcontrol) {}

	public override void Start ()
	{
		tPlayed = Time.time;
		tcontrol.DisplayMessage("Location unknown.. Attempting connection to main cluster.. ERROR 404\n" +
		                        "Scanning,.. Fleet Signal confirmed; Waypoint established.",  "Rob3", 10.0f);
	}

	public override void Update ()
	{

	}

	public override void Exit ()
	{
		tcontrol.NextState();
	}

	public override void GuiCallback ()
	{
		tcontrol.HideMessage();
	}
}

