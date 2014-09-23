using UnityEngine;
using System.Collections;

public class TutorialMiningLaserMenu : Menu
{

	MenuButton pb;

	private void ButtonPressed(int id) {
		Debug.Log("lawl i pressed thebuy butn");
	}

	public TutorialMiningLaserMenu(Rect area)
	{
		a = area;

		float xOff = a.width * 0.90f;
		float yOff = a.height * 0.95f;
		float btnWidth = a.width - xOff*1.01f;
		float btnHeight = a.height - yOff*1.001f;

		Rect btnrec = new Rect(a.x + xOff, a.y + yOff, btnWidth, btnHeight);
		pb = new MenuButton(btnrec, "buy", ButtonPressed);
		pb.SetDraw(true);

	}

	public override void gUpdate() { 
		if(!draw)
			return;

		GUI.Box(a, "");

		pb.gUpdate();

	}
}

