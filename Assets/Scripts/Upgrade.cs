using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class SerialUpgrade {
	public int id;
	public bool unlocked;
}

public abstract class Upgrade : MonoBehaviour {
	public string typeName;

	public bool bought;
	public int price;
	public Upgrade preReq;

	public int uniqueID; //Hopefully
	public string description;
	public string upgradeName;
	public Texture icon;

	string tooltip;

	public abstract void ApplyUpgrade(ShipComponent comp) ;

	public void UpdateTooltip() {
		tooltip = upgradeName + "\n" + (bought?"Already researched.":"Price: "+price) + "\n" + description;
	}

	public bool CanBeBought() {
		return (!bought&&(preReq==null||preReq.bought));
	}

	public int Buy() {
		if (!bought&&(preReq==null||preReq.bought)) {
			UpdateTooltip ();
			bought = true;
			return price;
		}
		return 0; 
	}

	public string Tooltip {
		get {
			return tooltip;
		}
		set {
			tooltip = value;
		}
	}

	public SerialUpgrade Save() {
		SerialUpgrade s = new SerialUpgrade();
		s.id = uniqueID;
		s.unlocked = bought;
		return s;
	}
}
