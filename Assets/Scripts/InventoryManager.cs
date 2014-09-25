using UnityEngine;
using System.Collections;
using System.Linq;

public class InventoryManager
{
	PlayerShip s;
	ArrayList weapons;
	ArrayList hulls;
	ArrayList engines;
	ArrayList cockpits;
	ArrayList wings;
	int[] minerals;
	int capacity;
	int currentLoad;

	static Object mutex = new Object();
	static InventoryManager instance;

	private InventoryManager() 
	{
		weapons 	= new ArrayList();
		hulls 		= new ArrayList();
		engines 	= new ArrayList();
		cockpits	= new ArrayList();
		wings		 	= new ArrayList();
		minerals	= Enumerable.Repeat(0,4).ToArray();
	
		currentLoad = 0;
		capacity = 1000;


		GameObject go = GameObject.Find("TestPlayerShipV2");
		s = go.GetComponent(typeof(PlayerShip)) as PlayerShip;

	}

	public bool AddMineral(MineralType t, int q) {
		if(currentLoad + q > capacity)
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
		if(!weapons.Contains(cockpit))
			weapons.Add(cockpit);
	}

	public void AddWings(Wings wing) {
		if(!wings.Contains(wing))
			wings.Add(wing);
	}

	public ArrayList GetWeapons() {
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

	public void Equip(Object item) {
		if(item is Weapon) {
			if(s.weapon != null)
				s.weapon.gameObject.SetActive(false);

			Weapon weapon = item as Weapon;
			GameObject weappos = GameObject.Find("WeaponPos");
			weapon.parent = s.gameObject;

			weapon.transform.parent = weappos.transform;
			weapon.transform.position = weappos.transform.position;

			weapon.gameObject.SetActive(true);
			s.weapon = weapon;
		} else if(item is Engine) {
			s.engine = item as Engine;
		} else if(item is Hull) {
			s.hull = item as Hull;
		}	else if(item is Wings) {
				s.wings = item as Wings;
		} else if(item is Cockpit) {
			s.cockpit = item as Cockpit;
		}
	}

	public static InventoryManager GetInstance() 
	{
		lock(mutex) 
		{
			if(instance == null) {
				instance = new InventoryManager();
			}
		}

		return instance;
	}
}