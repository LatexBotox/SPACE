using UnityEngine;
using System.Collections;

public class Tstate2 : TutorialState
{

	public Ship pShip;
	public GameObject motherShip;
	private Vector2 spawnPos;
	private bool foundShip = false;

	public override void Run ()
	{
		tControl.DisplayMessage("Location unknown.. Attempting connection to main cluster.. ERROR 404\n" +
		                        "Scanning,.. Fleet Signal confirmed; Waypoint established.",  "Rob3", 10.0f);
		spawnPos = pShip.transform.position + pShip.transform.up * 200;
		Instantiate(motherShip, spawnPos, Quaternion.identity);
		print (spawnPos);
	}

	public override void TriggerEnter(StateTrigger t, Collider2D c)
	{
		t.gameObject.SetActive(false);
		exit = true;
	}
}

