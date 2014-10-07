using UnityEngine;
using System.Collections;

public class Tstate5 : TutorialState
{
	public AsteroidGenerator ag;

	public override void Run ()
	{
		tControl.DisplayMessage("Nice! Scanners indicate an asteroid nearby. Go see if you can harvest anything useful from it." +
			" Hopefully we can use it to get to nearest star cluster.." ,"Captain Stenis", 10.0f);
		Asteroid a = ag.GenerateAsteroid(MineralType.Whatium, 2, Random.Range (int.MinValue, int.MaxValue));
		a.transform.position = new Vector3(0,0,0);
		a.gameObject.SetActive(true);
		PlayerShip.instance.weapon.enabled = true;
	}
	
	public override void sUpdate ()
	{
		if(InventoryManager.instance.GetLoad() > 0) {
			exit = true;
		}
	}
}