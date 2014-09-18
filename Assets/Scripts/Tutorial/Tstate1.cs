using UnityEngine;
using System.Collections;

public class Tstate1 : TutorialState
{

	public Tstate1(ShipController _ship, TutorialController _tcontrol) : base(_ship, _tcontrol) {}

	public override void Start ()
	{
		tcontrol.DisplayMessage("System online; Commence main engine test. (Default key: W)", "Rob", 10.0f);
	}

	public override void Update ()
	{
	}

	public override void Exit ()
	{
		throw new System.NotImplementedException ();
	}

	public override void GuiCallback ()
	{
	}
}

