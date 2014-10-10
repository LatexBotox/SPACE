using UnityEngine;
using System.Collections;

public class EngineUpg : Upgrade {
	public float maxMult = 0.1f;
	public float accMult = 0.12f;

	public void Reset() {
		typeName = "Engine";
		upgradeName = "Engine Level 1";
		description = "Increases max speed by 10%. Increases acceleration by 12%.";
	}

	public override void ApplyUpgrade (ShipComponent comp)
	{
		Engine e = comp.GetComponent (typeName) as Engine;
		if(!e)
			return;

		e.maxSpeed *= 1+maxMult;
		e.thrustMult *= 1+accMult;
	}
}
