using UnityEngine;
using System.Collections;

public class Tstate5 : TutorialState
{
	public AsteroidGenerator ag;

	public override void Run ()
	{
		Asteroid a = ag.GenerateAsteroid(MineralType.Whatium, 2, Random.Range (int.MinValue, int.MaxValue));
		a.transform.position = (Vector2)PlayerShip.instance.transform.position + Random.insideUnitCircle * 300f;
		a.gameObject.SetActive(true);
	}
	
	public override void sUpdate ()
	{
		if(InventoryManager.instance.GetLoad() > 0) {
			exit = true;
		}
	}
}