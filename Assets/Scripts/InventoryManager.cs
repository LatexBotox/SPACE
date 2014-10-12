using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Linq;

public class InventoryManager : MonoBehaviour
{
	int[] minerals = new int[4];
	int currentLoad = 0;
	int loadCapacity = 1000;

    int currency;

	public static InventoryManager instance;

	void Start() {
		if (instance !=null) {
			Destroy (gameObject, 0f);
			return;
		}

		instance = this;
		DontDestroyOnLoad(gameObject);
	}

	public bool AddMineral(MineralType t, int q) {
		if(currentLoad + q > loadCapacity)
			return false;

		switch(t) {
		case MineralType.Copperium :
			minerals[0] += q;
			break;
		case MineralType.Gallium :
			minerals[1] += q;
			break;
		case MineralType.Iron :
			minerals[2] += q;
			break;
		case MineralType.Whatium :
			minerals[3] += q;
			break;
		default :
			return false;
		}

		currentLoad += q;
		return true;
	}

    public void MineralsToCurrency() {
        for (int i = 0; i < minerals.Length; i++)
        {
            currency += i * minerals[i];
            minerals[i] = 0;
        }
    }


	public int GetLoad() { return currentLoad; }
	public int GetMaxLoad() { return loadCapacity; }

	public void Equip(Object item) {
		if(item is Weapon) {
		
			Weapon clone = Instantiate(item, new Vector2(0,0), Quaternion.identity) as Weapon; 
			PlayerShip.instance.SetWeapon(clone);
		} else if(item is Engine) {
			PlayerShip.instance.engine = item as Engine;
		} else if(item is Hull) {
			PlayerShip.instance.hull = item as Hull;
		}	else if(item is Wings) {
			PlayerShip.instance.wings = item as Wings;
		} else if(item is Cockpit) {
			PlayerShip.instance.cockpit = item as Cockpit;
		} else {
			Debug.LogError("invalid type: " + item.GetType());		
		}
	}
}