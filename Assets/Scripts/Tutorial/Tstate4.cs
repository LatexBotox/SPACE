using UnityEngine;
using System.Collections;

public class Tstate4 : TutorialState
{
	public CameraHandler camHandle;

	public override void Run ()
	{
		camHandle.ActivateHangarCam();
		tControl.DisplayMessage("Install the mining laser so that i can FRAPE YOOUUU", "Captain Stenis", 10.0f);
	}

	public override void sUpdate ()
	{
		if(Stuff.inventory.GetWeapons().Count > 0) {
			exit = true;
		}
	}
}

