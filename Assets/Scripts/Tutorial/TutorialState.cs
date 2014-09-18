using UnityEngine;
using System.Collections;

public abstract class TutorialState
{

	protected ShipController ship;
	protected TutorialController tcontrol;

	public TutorialState(ShipController _ship, TutorialController _tcontrol) {
		ship = _ship;
		tcontrol = _tcontrol;
	}

	public abstract void Start();
	public abstract void Update();
	public abstract void Exit();
	public abstract void GuiCallback();
}

