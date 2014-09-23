using UnityEngine;
using System.Collections;

public class Tstate4 : TutorialState
{
	public PlayerShip pl;
	public CameraHandler camHandle;

	public override void Run ()
	{
		pl.gameObject.SetActive(false);
		camHandle.ActivateHangarCam();
		tControl.DisplayMessage("Install the mining laser so that i can FRAPE YOOUUU", "Captain Stenis", 10.0f);
	}
}

