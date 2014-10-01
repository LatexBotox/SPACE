using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShip : Ship
{
	//public CustomInput input;	

	KeyListener thrust, rotatel, rotater, dampen, shoot;

	public static PlayerShip instance;

	protected override void Start () {
		//base.Start ();
		//cockpit.StartRadar ();
		if(instance!=null) {
			Destroy (gameObject, 0f);
			return;
		}
		instance = this;

		thrust 	= new KeyListener(CustomInput.instance.ThrustKey, 	this.ThrustEngines);
		rotatel = new KeyListener(CustomInput.instance.RotateLKey, this.RotateLeft);
		rotater = new KeyListener(CustomInput.instance.RotateRKey, this.RotateRight);
		dampen 	= new KeyListener(CustomInput.instance.DampenKey, 	this.Dampen);
		shoot 	= new KeyListener(CustomInput.instance.ShootKey, 	this.FireWeapons);

		CustomInput.instance.RegHeldListener(thrust);
		CustomInput.instance.RegHeldListener(rotatel);
		CustomInput.instance.RegHeldListener(rotater);
		CustomInput.instance.RegHeldListener(dampen);
		CustomInput.instance.RegHeldListener(shoot);
	}
	
	public override void Init() {
		base.Init ();
		cockpit.StartRadar ();
	}

	void OnEnable() {
		if(weapon == null) {
			List<Weapon> weaps = InventoryManager.instance.GetWeapons();
			if(weaps.Count > 0)
				InventoryManager.instance.Equip(weaps[0] as Weapon);
		}
	}


	protected override void FixedUpdate ()
	{
		base.FixedUpdate ();
		Camera.main.orthographicSize = Mathf.Lerp (45, 55, rigidbody2D.velocity.magnitude/150);

		RotateWeapons (Camera.main.ScreenToWorldPoint (Input.mousePosition));

	}

	public override void Die ()
	{
		instance = null;
		base.Die ();
	}
}

