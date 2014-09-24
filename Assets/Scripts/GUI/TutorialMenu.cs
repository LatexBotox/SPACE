using UnityEngine;
using System.Collections;

public class TutorialMenu : Menu
{
	TutorialWeaponMenu wm;
	MenuButton weapBtn;
	MenuButton exitBtn;
	const int nr_items = 1;

	private void ButtonPressed(int id) {
		if(id == weapBtn.Id) {
			wm.SetDraw (!wm.draw);
		} else if(id == exitBtn.Id) {
			CameraHandler camhandl = GameObject.Find("CameraHandler").GetComponent<CameraHandler>();
			camhandl.ActivateMainCam();
		}
	}


	public TutorialMenu (Rect area)
	{
		a = area;
		wm = new TutorialWeaponMenu(new Rect(a.width, 0, a.width, a.height));
		wm.SetDraw(false);

		float xOff = 0.15f * a.width;
		float yOff = 0.02f * a.height;
		float btnWidth = a.width - 2*xOff;
		float btnHeight = 0.05f * a.height; 

		Rect btnrec = new Rect(a.x + xOff, a.y + yOff, btnWidth, btnHeight);  

		weapBtn = new MenuButton(btnrec, "weapons", ButtonPressed);
		weapBtn.SetDraw(true);

		btnrec = new Rect(btnrec.x, btnrec.y + btnHeight + yOff, btnWidth, btnHeight);
		exitBtn = new MenuButton(btnrec, "exit", ButtonPressed);
		exitBtn.SetDraw(true);
	}
	
	public override void gUpdate ()
	{
		if(!draw)
			return;

		GUI.Box(a, "");

		wm.gUpdate ();
		weapBtn.gUpdate();
		exitBtn.gUpdate();
	}
}

