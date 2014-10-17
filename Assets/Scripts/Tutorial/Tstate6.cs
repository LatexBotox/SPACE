using UnityEngine;
using System.Collections;

public class Tstate6 : TutorialState
{
	public EnemyShip enemy;

	EnemyShip clone;

	public override void Run ()
	{
		Vector2 spawnpos = (Vector2)PlayerShip.instance.transform.position + (Vector2)Random.insideUnitCircle * 300f;
		clone = Instantiate(enemy, spawnpos, Quaternion.identity) as EnemyShip;
		clone.target = PlayerShip.instance;
		Message m = new Message ("Ship detected! Closing in fast!\n Their signature reads.. Oh no.. \nSPACEPIRATES! Save me mommy!", "Captn S");
		Messenger.instance.AddMessage (m);
	}
	
	public override void sUpdate ()
	{
		if(clone == null) {
			exit = true;
		}
	}
}

