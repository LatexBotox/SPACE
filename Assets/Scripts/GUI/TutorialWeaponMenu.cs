using UnityEngine;
using System.Collections;

public class TutorialWeaponMenu : Menu
{

	TutorialMiningLaserMenu tmlm;
	MenuButton mlb;

	private void ButtonPressed(int id) {
		if(id == mlb.Id) {
			tmlm.SetDraw(!tmlm.draw);
		}
	}

	public TutorialWeaponMenu(Rect area)
	{
		a = area;		
		tmlm = new TutorialMiningLaserMenu(new Rect(a.x + a.width, a.y, Screen.width - (a.x + a.width), a.height));
		tmlm.SetDraw(false);

		float xOff = 0.0f;
		float yOff = 0.0f;
		float btnWidth = a.width - 2*xOff;
		float btnHeight = 0.05f * a.height; 
		
		Rect btnrec = new Rect(a.x + xOff, a.y + yOff, btnWidth, btnHeight);  
		mlb = new MenuButton(btnrec, "mining laser", ButtonPressed);
		mlb.SetDraw(true);
	}


	public override void gUpdate ()
	{
		if(!draw)
			return;

		GUI.Box(a, "");

		mlb.gUpdate();
		tmlm.gUpdate();

	}
}

