using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Linq;

public class InventoryManager : MonoBehaviour
{
	public Weapon[] weaps;// 	= new ArrayList();

	List<Weapon> weapons = new List<Weapon>();
	ArrayList hulls 		= new ArrayList();
	ArrayList engines 	= new ArrayList();
	ArrayList cockpits 	= new ArrayList();
	ArrayList wings 		= new ArrayList();
	int[] minerals = new int[4];
	int currentLoad = 0;
	int loadCapacity = 1000;

	public static InventoryManager instance;

	void Start() {
		if (instance !=null) {
			Destroy (gameObject, 0f);
			return;
		}

		instance = this;
		DontDestroyOnLoad(gameObject);
		weapons.AddRange(weaps);
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

	public void AddWeapon(Weapon weap) {
		if(!weapons.Contains(weap))
			weapons.Add(weap);
	}
	
	public void AddHull(Hull hull) {
		if(!hulls.Contains(hull))
			hulls.Add(hull);
	}
	
	public void AddEngine(Engine engine) {
		if(!engines.Contains(engine))
			engines.Add(engine);
	}
	
	public void AddCockpit(Cockpit cockpit) {
		if(!cockpits.Contains(cockpit))
			cockpits.Add(cockpit);
	}

	public void AddWings(Wings wing) {
		if(!wings.Contains(wing))
			wings.Add(wing);
	}

	public List<Weapon> GetWeapons() {
		return weapons;
	}

	public ArrayList GetHulls() {
		return hulls;
	}
	
	public ArrayList GetEngines() {
		return engines;
	}

	public ArrayList GetCockpits() {
		return cockpits;
	}

	public ArrayList GetWings() {
		return wings;
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