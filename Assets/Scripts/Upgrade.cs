using UnityEngine;
using System;
using System.Collections;

public abstract class Upgrade : MonoBehaviour {
	public string typeName;
	public int maxRank;
	public int rank;

	public int[] upgradeCost;
	public string tooltip;
	public Texture icon;


	public abstract void ApplyUpgrade(ShipComponent comp) ;

}
