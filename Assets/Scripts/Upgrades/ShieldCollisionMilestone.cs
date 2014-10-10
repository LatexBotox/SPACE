using UnityEngine;
using System.Collections;

public class ShieldCollisionMilestone : Upgrade
{
	public float colSelfMult = 0.25f;
	public float colVictimMult = 0.5f;

	public void Reset() {
		typeName = "Wings";
		upgradeName = "Shield Collision";
		description = "Reduces damage taken from collisions while shields are up by 25%. " +
			"Increases damage enemies take from colliding with you when your shields are up by 50%.";
	}
	
	public override void ApplyUpgrade (ShipComponent comp) {
		Wings w = comp.GetComponent (typeName) as Wings;
		if(!w)
			return;
		
		w.colSelfMult = 1-colSelfMult;
		w.colVictimMult = 1+colVictimMult;
	}
}

