using UnityEngine;
using System.Collections;

public class HullUpgrade : Upgrade
{
	public float healthPerRank = 0.5f;
	public float capacityPerRank = 1;

	public void Reset() {
		typeName = "Hull";
		rank = 0;
		maxRank = 5;
		upgradeCost = new int[]{100,200,300,400,500};
		tooltip = "Hull Upgrade\nRank "+rank+"/"+maxRank+"\nUpgrade Cost: "+(rank==maxRank?"Maxed":upgradeCost[rank].ToString())+
			"\nHealth increased by 50% per rank.\nCarrying capacity increased by 100% per rank.";
	}
	
	public override void ApplyUpgrade (ShipComponent comp)
	{
		Hull h = comp.GetComponent (typeName) as Hull;
		if(!h)
			return;

		h.maxHealth *= 1+(healthPerRank*rank);
		h.maxCapacity *= 1+(capacityPerRank*rank);
	}
}

