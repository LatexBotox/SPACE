using UnityEngine;
using System.Collections;

public enum MineralType {
	Iron,
	Copperium,
	Gallium,
	Whatium,
	Blank
}

public class Mineral: Pickup
{
	public MineralType type;
	public int quantity;
	
	protected override void Pick ()
	{
		if(InventoryManager.GetInstance().AddMineral(type, quantity)) {
			Destroy(this.gameObject, 0.0f);
		}
	}
}

