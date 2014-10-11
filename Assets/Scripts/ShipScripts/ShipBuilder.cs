using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class SerialShip {
	public int[] unlockedParts;
	public SerialUpgrade[] upgrades;
	public int wingIndex, engineIndex, weaponIndex, hullIndex, cockpitIndex;
	public int wingColor, engineColor, weaponColor, hullColor, cockpitColor;
}

public class ShipBuilder : MonoBehaviour {
	public Wings[] 		wings;
	public Engine[] 	engines;
	public Weapon[] 	weapons;
	public Hull[] 		hulls;
	public Cockpit[]	cockpits;

	public Upgrade[] upgrades;

	public Ship shipBase;

	public int wingIndex, engineIndex, weaponIndex, hullIndex, cockpitIndex = 0;

	public Color wingColor;
	public Color engineColor;
	public Color hullColor;
	public Color weaponColor;
	public Color cockpitColor;

	public bool waitForInput = true;

	public Wings 	selectedWing;
	public Engine 	selectedEngine;
	public Weapon 	selectedWeapon;
	public Hull 		selectedHull;
	public Cockpit selectedCockpit;

	public static ShipBuilder instance;

	public List<int> boughtParts;
	public List<int> unlockedParts;

	void Start () {
		if (instance!=null) {
			Destroy (gameObject, 0f);
			return;
		}
		instance = this;

		int i=0;
		foreach (Upgrade u in upgrades) {
			u.uniqueID = i++;
			u.UpdateTooltip();
		}

		unlockedParts = new List<int>();
		i=0;
		foreach (ShipComponent s in wings){
			s.uniqueID = i++;
			unlockedParts.Add (i);
		}
		foreach (ShipComponent s in engines){
			s.uniqueID = i++;
			unlockedParts.Add (i);
		}
		foreach (ShipComponent s in weapons){
			s.uniqueID = i++;
			unlockedParts.Add (i);
		}
		foreach (ShipComponent s in hulls){
			s.uniqueID = i++;
			unlockedParts.Add (i);
		}
		foreach (ShipComponent s in cockpits){
			s.uniqueID = i++;
			unlockedParts.Add (i);
		}

		boughtParts = new List<int>();

		print (Application.persistentDataPath);

		if (!Load ()) {
			cockpitIndex 	= 0;
			engineIndex 	= 0;
			hullIndex 		= 0;
			weaponIndex 	= 0;
			wingIndex 		= 0;

			UpdateSelected ();

			boughtParts.Add (selectedCockpit.uniqueID);
			boughtParts.Add (selectedEngine.uniqueID);
			boughtParts.Add (selectedHull.uniqueID);
			boughtParts.Add (selectedWeapon.uniqueID);
			boughtParts.Add (selectedWing.uniqueID);

			cockpitColor 	= Color.white;
			engineColor 	= Color.white;
			hullColor 		= Color.white;
			weaponColor 	= Color.white;
			wingColor 		= Color.white;
			
			foreach(Upgrade u in upgrades)
				u.bought = false;
		} else {
			UpdateSelected ();
		}
		
		if(!waitForInput)
			SpawnShip();
	}

	void UpdateSelected() {
		selectedWing 		= wings[wingIndex];
		selectedEngine 	= engines[engineIndex];
		selectedWeapon 	= weapons[weaponIndex];
		selectedHull 		= hulls[hullIndex];
		selectedCockpit = cockpits[cockpitIndex];
	}

	public Ship SpawnShip() {
		UpdateSelected ();

		Ship ship 			= Instantiate (shipBase, transform.position, shipBase.transform.rotation) as Ship;
		Wings wing 			= Instantiate (selectedWing, transform.position+selectedWing.transform.position, selectedWing.transform.rotation) as Wings;
		Engine engine 	= Instantiate (selectedEngine, transform.position+selectedEngine.transform.position, selectedEngine.transform.rotation) as Engine;
		Weapon weapon 	= Instantiate (selectedWeapon, ship.weapPos.position, selectedWeapon.transform.rotation) as Weapon;
		Hull hull 			= Instantiate (selectedHull, transform.position+selectedHull.transform.position, selectedHull.transform.rotation) as Hull;
		Cockpit cockpit = Instantiate (selectedCockpit, transform.position+selectedCockpit.transform.position, selectedCockpit.transform.rotation) as Cockpit;

		wing.transform.parent 		= ship.transform;
		engine.transform.parent 	= ship.transform;
		weapon.transform.parent 	= ship.weapPos.transform;
		hull.transform.parent 		= ship.transform;
		cockpit.transform.parent 	= ship.transform;
		
		wing.renderer.material.SetColor("_PaintColor", wingColor);
		engine.renderer.material.SetColor("_PaintColor", engineColor);
		weapon.renderer.material.SetColor("_PaintColor", weaponColor);
		hull.renderer.material.SetColor("_PaintColor", hullColor);
		cockpit.renderer.material.SetColor("_PaintColor", cockpitColor);

		foreach (ShipComponent c in ship.GetComponentsInChildren<ShipComponent>())
			ApplyUpgrades (c);

		ship.Init ();
		engine.Init ();
		weapon.Init ();

		return ship;
	}

	public void DespawnShip() {
		PlayerShip.instance.Destroy();
	}

	void ApplyUpgrades(ShipComponent c) {
		foreach (Upgrade u in upgrades) {
			if (c.GetComponent(u.typeName)&&u.bought) {
				u.ApplyUpgrade(c);
			}
		}
	}

	public bool IsBought(int id) {
		return (boughtParts.Contains (id));
	}

	public bool IsUnlocked(int id) {
		return (unlockedParts.Contains (id));
	}

	public void Buy(int id) {
		boughtParts.Add (id);
	}

    public void Equip(int id)
    {
        int index = -1;
        if ((index = Array.FindIndex<Cockpit>(cockpits, c => c.uniqueID == id))>-1)
        {
            cockpitIndex = index;
        }
        else if ((index = Array.FindIndex<Engine>(engines, e => e.uniqueID == id)) > -1)
        {
            engineIndex = index;
        }
        else if ((index = Array.FindIndex<Hull>(hulls, h => h.uniqueID == id)) > -1)
        {
            hullIndex = index;
        }
        else if ((index = Array.FindIndex<Weapon>(weapons, w => w.uniqueID == id)) > -1)
        {
            weaponIndex = index;
        }
        else if ((index = Array.FindIndex<Wings>(wings, w => w.uniqueID == id)) > -1)
        {
            wingIndex = index;
        }

        UpdateSelected();
    }

	public void ChangeWeapon(Weapon w) {
		if (boughtParts.IndexOf(w.uniqueID)>-1) 
			PlayerShip.instance.SetWeapon (w).renderer.material.SetColor("_PaintColor",weaponColor);
	}

	public bool Load() {
		if (File.Exists (Application.persistentDataPath + "/ship.ass")) {
			print ("Loading Ship...");
			BinaryFormatter bf = new BinaryFormatter();
			FileStream fs = File.Open (Application.persistentDataPath + "/ship.ass", FileMode.Open);

			SerialShip saveData = (SerialShip)bf.Deserialize (fs);
			fs.Close();
			
			foreach(int i in saveData.unlockedParts)
				boughtParts.Add (i);

			for(int i=0;i<upgrades.Length;i++)
				upgrades[i].bought = saveData.upgrades[i].unlocked;

			cockpitColor 	= unshittyColor(saveData.cockpitColor);
			engineColor 	= unshittyColor(saveData.engineColor);
			hullColor 		= unshittyColor(saveData.hullColor);
			weaponColor 	= unshittyColor(saveData.weaponColor);
			wingColor 		= unshittyColor(saveData.wingColor);

			cockpitIndex 	= saveData.cockpitIndex;
			engineIndex 	= saveData.engineIndex;
			hullIndex 		= saveData.hullIndex;
			weaponIndex 	= saveData.weaponIndex;
			wingIndex 		= saveData.wingIndex;

			return true;
		}

		return false;
	}

	public void Save() {
		SerialShip saveData 	= new SerialShip();
		int[] partData 				= new int[boughtParts.Count];
		SerialUpgrade[] upgradeData = new SerialUpgrade[upgrades.Length];

		saveData.cockpitColor = shittyColor (cockpitColor);
		saveData.engineColor	= shittyColor (engineColor);
		saveData.hullColor		= shittyColor (hullColor);
		saveData.weaponColor	= shittyColor (weaponColor);
		saveData.wingColor		= shittyColor (wingColor);

		saveData.cockpitIndex = cockpitIndex;
		saveData.engineIndex 	= engineIndex;
		saveData.hullIndex 		= hullIndex;
		saveData.weaponIndex 	= weaponIndex;
		saveData.wingIndex 		= wingIndex;

		int i = 0;
		foreach (Upgrade u in upgrades)
			upgradeData[i++] = u.Save();

		i = 0;
		foreach (int j in boughtParts) 
			partData[i++] = j;

		saveData.unlockedParts 	= partData;
		saveData.upgrades				= upgradeData;

		BinaryFormatter bf = new BinaryFormatter();
		
		FileStream fs = File.Create (Application.persistentDataPath + "/ship.ass");
		
		bf.Serialize (fs, saveData);
		fs.Close ();
	}

	public void SetAllColors(Color c) {
		cockpitColor = engineColor = hullColor = weaponColor = wingColor = c;
	}

	int shittyColor(Color c) {
		int s = 0;

		s += Mathf.FloorToInt(c.r*100);
		s *= 1000;
		s += Mathf.FloorToInt(c.g*100);
		s *= 1000;
		s += Mathf.FloorToInt(c.b*100);

		print ("Shitty Color: " + s);

		return s;
	}

	Color unshittyColor(int i) {
		Color c = new Color();

		c.a = 1;

        print("Color Unshittied: " + i);

		c.r = Mathf.FloorToInt(i/1000000)/100f;
		i -= Mathf.FloorToInt(i/1000000)*1000000;
		c.g = Mathf.FloorToInt(i/1000)/100f;
		i -= Mathf.FloorToInt(i/1000)*1000;
		c.b = i/100f;

		print ("Gives: " + c.r + ", " + c.g + ", " + c.b);

		return c;
	}
}
