using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipBuilder : MonoBehaviour {
	public Wings[] 	wings;
	public Engine[] 	engines;
	public Weapon[] 	weapons;
	public Hull[] 		hulls;
	public Cockpit[]	cockpits;

	public Ship shipBase;

	public int wingIndex, engineIndex, weaponIndex, hullIndex, cockpitIndex;

	public Color wingColor;
	public Color engineColor;
	public Color hullColor;
	public Color weaponColor;
	public Color cockpitColor;

	Wings 	selectedWing;
	Engine 	selectedEngine;
	Weapon 	selectedWeapon;
	Hull 		selectedHull;
	Cockpit selectedCockpit;

	public static ShipBuilder instance;

	SortedList<string, bool> unlocked;
	void Start () {
		if (instance!=null) {
			Destroy (gameObject, 0f);
			return;
		}
		instance = this;

		unlocked = new SortedList<string, bool>();
		foreach(ShipComponent s in wings)
			unlocked.Add (s.name, true);
		foreach(ShipComponent s in engines)
			unlocked.Add (s.name, true);
		foreach(ShipComponent s in weapons)
			unlocked.Add (s.name, true);
		foreach(ShipComponent s in hulls)
			unlocked.Add (s.name, true);
		foreach(ShipComponent s in cockpits)
			unlocked.Add (s.name, true);
		selectedWing = wings[wingIndex];
		selectedEngine = engines[engineIndex];
		selectedWeapon = weapons[weaponIndex];
		selectedHull = hulls[hullIndex];
		selectedCockpit = cockpits[cockpitIndex];

		SpawnShip();
	}

	public Ship SpawnShip() {
		Ship ship = Instantiate (shipBase, transform.position, shipBase.transform.rotation) as Ship;
		Wings wing = Instantiate (selectedWing, transform.position+selectedWing.transform.position, selectedWing.transform.rotation) as Wings;
		Engine engine = Instantiate (selectedEngine, transform.position+selectedEngine.transform.position, selectedEngine.transform.rotation) as Engine;
		Weapon weapon = Instantiate (selectedWeapon, ship.weapPos.position, selectedWeapon.transform.rotation) as Weapon;
		Hull hull = Instantiate (selectedHull, transform.position+selectedHull.transform.position, selectedHull.transform.rotation) as Hull;
		Cockpit cockpit = Instantiate (selectedCockpit, transform.position+selectedCockpit.transform.position, selectedCockpit.transform.rotation) as Cockpit;

		wing.transform.parent = ship.transform;
		engine.transform.parent = ship.transform;
		weapon.transform.parent = ship.weapPos.transform;
		hull.transform.parent = ship.transform;
		cockpit.transform.parent = ship.transform;

		wing.renderer.material.SetColor("_PaintColor", wingColor);
		engine.renderer.material.SetColor("_PaintColor", engineColor);
		weapon.renderer.material.SetColor("_PaintColor", weaponColor);
		hull.renderer.material.SetColor("_PaintColor", hullColor);
		cockpit.renderer.material.SetColor("_PaintColor", cockpitColor);

		ship.Init ();
		engine.Init ();
		weapon.Init ();

		return ship;
	}

	public void ChangeWeapon(Weapon w) {
		if (unlocked[w.name]) {
			PlayerShip.instance.SetWeapon (w).renderer.material.SetColor("_PaintColor",weaponColor);
		}
	}
}
