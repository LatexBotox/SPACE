using UnityEngine;
using System.Collections;

public static class Stuff
{
	public static PlayerShip player;
	public static InventoryManager inventory;
	public static CustomInput input;

	static Stuff() {
		GameObject g;

		g = FindGameobj("InventoryManager");
		inventory = FindComponent(g, typeof(InventoryManager)) as InventoryManager;

		g = FindGameobj("TestPlayerShipV2");
		player = FindComponent(g, typeof(PlayerShip)) as PlayerShip;

		g = FindGameobj("CustomInput");
		input = FindComponent(g, typeof(CustomInput)) as CustomInput;
	}

	private static GameObject FindGameobj(string name) 
	{
		GameObject ret = GameObject.Find(name);
		if(ret == null)
			Stuff.Error("Could not locate gameobj: " + name);

		return ret;

	}

	private static Component FindComponent(GameObject g, System.Type t)
	{
		Component ret = g.GetComponent(t);
		if(ret == null)
			Stuff.Error ("Could not locate component of type: " + t);

		return ret;
	}

	private static void Error(string msg) {
		Debug.LogError("Error: " + msg);
	}
}

