using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponSelector : MonoBehaviour 
{
	int wid;
	KeyListener next, prev;


	void Start() {
		wid = 0;
		next = new KeyListener(Stuff.input.NextWeaponKey, this.NextWeapon, 0.2f);
		prev = new KeyListener(Stuff.input.PrevWeaponKey, this.PrevWeapon, 0.2f);
		Stuff.input.RegDownListener(next);
		Stuff.input.RegDownListener(prev);
	}


	void NextWeapon() {
		List<Weapon> w = Stuff.inventory.GetWeapons();
		wid = (++wid)%w.Count;
	
		foreach(Weapon weap in w) {
			print (weap.name);
		}


		Stuff.inventory.Equip(w[wid] as Weapon);
	}

	void PrevWeapon() {
		List<Weapon> w = Stuff.inventory.GetWeapons();
		wid = (++wid)%w.Count;
		Stuff.inventory.Equip(w.ToArray ()[wid] as Weapon);
	}
}
