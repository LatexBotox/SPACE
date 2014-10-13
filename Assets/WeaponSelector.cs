using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponSelector : MonoBehaviour 
{
	KeyListener next, prev;


	void Start() {



	}

	void Update() {
		try {
			next = new KeyListener(CustomInput.instance.NextWeaponKey, this.NextWeapon, 0.2f);
			prev = new KeyListener(CustomInput.instance.PrevWeaponKey, this.PrevWeapon, 0.2f);
			CustomInput.instance.RegDownListener(next);
			CustomInput.instance.RegDownListener(prev);
			enabled = false;
		} catch (UnityException e) {
			Debug.LogException(e);
		} 
	
	}

	void NextWeapon() {
		print ("lawl");
		ShipBuilder.instance.weaponIndex = (ShipBuilder.instance.weaponIndex+1)%ShipBuilder.instance.weapons.Length;

		ShipBuilder.instance.ChangeWeapon(ShipBuilder.instance.weapons[ShipBuilder.instance.weaponIndex]);
	}

	void PrevWeapon() {
		if (ShipBuilder.instance.weaponIndex == 0) {
			ShipBuilder.instance.weaponIndex = ShipBuilder.instance.weapons.Length-1;
		} else {
			ShipBuilder.instance.weaponIndex--;
		}
		
		ShipBuilder.instance.ChangeWeapon(ShipBuilder.instance.weapons[ShipBuilder.instance.weaponIndex]);
	}
}
