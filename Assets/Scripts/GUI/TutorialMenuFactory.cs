using UnityEngine;
using System.Collections;

public class TutorialMenuFactory : MenuFactory
{
	
	public override Menu CreateMenu ()
	{
		return new TutorialMenu(new Rect(0,0, Screen.width / 6.0f, Screen.height));
	}

}