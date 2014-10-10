using UnityEngine;
using System.Collections;

public class HullUpg : Upgrade
{
	public float healthMult = 0.5f;
	public float capacityMult = 1;

	public void Reset() {
		typeName = "Hull";
		description = "Increases health by 50%. Increases carrying capacity by 100%.";
		UpdateTooltip();
	}
	
	public override void ApplyUpgrade (ShipComponent comp)
	{
		Hull h = comp.GetComponent (typeName) as Hull;
		if(!h)
			return;

		h.maxHealth *= 1+healthMult;
		h.maxCapacity *= 1+capacityMult;
	}
}

