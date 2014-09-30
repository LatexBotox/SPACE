using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShip : Ship
{
	//public CustomInput input;	

	KeyListener thrust, rotatel, rotater, dampen, shoot;

	protected override void Start () {
		//base.Start ();
		//cockpit.StartRadar ();

		if(weapon != null) 
		{
			Stuff.inventory.AddWeapon(weapon);
		}

		thrust 	= new KeyListener(Stuff.input.ThrustKey, 	this.ThrustEngines);
		rotatel = new KeyListener(Stuff.input.RotateLKey, this.RotateLeft);
		rotater = new KeyListener(Stuff.input.RotateRKey, this.RotateRight);
		dampen 	= new KeyListener(Stuff.input.DampenKey, 	this.Dampen);
		shoot 	= new KeyListener(Stuff.input.ShootKey, 	this.FireWeapons);

		Stuff.input.RegHeldListener(thrust);
		Stuff.input.RegHeldListener(rotatel);
		Stuff.input.RegHeldListener(rotater);
		Stuff.input.RegHeldListener(dampen);
		Stuff.input.RegHeldListener(shoot);
	}
	
	public override void Init() {
		base.Init ();
		cockpit.StartRadar ();

	}

	void OnEnable() {
		if(weapon == null) {
			List<Weapon> weaps = Stuff.inventory.GetWeapons();
			if(weaps.Count > 0)
				Stuff.inventory.Equip(weaps[0] as Weapon);
		}
	}


	protected override void FixedUpdate ()
	{
		base.FixedUpdate ();
		Camera.main.orthographicSize = Mathf.Lerp (45, 55, rigidbody2D.velocity.magnitude/150);

		RotateWeapons (Camera.main.ScreenToWorldPoint (Input.mousePosition));

	}
}

