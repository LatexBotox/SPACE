using UnityEngine;
using System.Collections;

public class Tstate5 : TutorialState
{
	public AsteroidGenerator ag;

	public override void Run ()
	{
		tControl.DisplayMessage("Nice! Now go fetch resources!" ,"Captain Stenis", 10.0f);
		Asteroid a = ag.GenerateAsteroid(MineralType.Whatium, 2, Random.Range (int.MinValue, int.MaxValue));
		a.transform.position = new Vector3(0,0,0);
		a.gameObject.SetActive(true);
	}
	
	public override void sUpdate ()
	{
		if(InventoryManager.GetInstance().GetLoad() > 0) {
			exit = true;
		}
	}
}