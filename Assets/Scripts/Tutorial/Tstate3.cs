using UnityEngine;
using System.Collections;

public class Tstate3 : TutorialState
{
	public override void TriggerEnter(StateTrigger t, Collider2D c) {
		exit = true;		
	}

	public override void Run ()
	{
		tControl.DisplayMessage("Imma gay for you Lowe! Please park your junk \nin mah trunk, aaaaalll the way up there", "Captain Stenis", 10.0f);
	}
}

