using UnityEngine;
using System.Collections;

public class Tstate2 : TutorialState
{
	public PlayerShip pship;

	public override void Run ()
	{
		pship=PlayerShip.instance;
		tControl.DisplayMessage("Location unknown.. Attempting connection to main cluster.. ERROR 404\n" +
		                        "Scanning,.. Fleet Signal confirmed; Waypoint established.",  "Rob", 10.0f);


		Vector2 pos = (Vector2)pship.transform.position + Random.insideUnitCircle.normalized * 200;
		Quaternion rot = pship.transform.rotation;
		tControl.SpawnMothership(pos, rot);
	}

	public override void TriggerEnter(StateTrigger t, Collider2D c)
	{
		t.gameObject.SetActive(false);
		exit = true;
	}
}

