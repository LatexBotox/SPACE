using UnityEngine;
using System.Collections;

public class ShieldCollisionMilestone : Upgrade
{
	public float colSelfPerRank = 0.25f;
	public float colVictimPerRank = 0.5f;

	public void Reset() {
		typeName = "Wings";
		rank = 0;
		maxRank = 2;
		upgradeCost = new int[]{400,800};
	}
	
	public override void ApplyUpgrade (ShipComponent comp)
	{
		Wings w = comp.GetComponent (typeName) as Wings;
		if(!w)
			return;
		
		w.colSelfMult = 1-(colSelfPerRank*rank);
		w.colVictimMult = 1+(colVictimPerRank*rank);
	}
}

