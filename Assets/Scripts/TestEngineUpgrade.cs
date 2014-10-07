using UnityEngine;
using System.Collections;

public class TestEngineUpgrade : Upgrade {
	public float maxPerRank = 0.1f;
	public float accPerRank = 0.12f;

	public void Reset() {
		typeName = "Engine";
		rank = 0;
		maxRank = 5;
		upgradeCost = new int[]{100,200,300,400,500};
	}

	public override void ApplyUpgrade (ShipComponent comp)
	{
		Engine e = comp.GetComponent (typeName) as Engine;
		if(!e)
			return;

		e.maxSpeed *= 1+(maxPerRank*rank);
		e.thrustMult *= 1+(accPerRank*rank);
	}
}
