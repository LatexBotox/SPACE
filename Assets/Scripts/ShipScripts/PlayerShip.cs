using UnityEngine;
using System.Collections;

public class PlayerShip : Ship
{
	public CustomInput input;	

	protected override void Start () {
		base.Start ();
		cockpit.StartRadar ();
	}


	void OnEnable() {
		if(weapon == null) {
			ArrayList weaps = InventoryManager.GetInstance().GetWeapons();
			if(weaps.Count > 0)
				InventoryManager.GetInstance().Equip(weaps[0] as Weapon);
		}
				
	}


	protected override void FixedUpdate ()
	{
		base.FixedUpdate ();
		Camera.main.orthographicSize = Mathf.Lerp (45, 55, rigidbody2D.velocity.magnitude/150);

		RotateWeapons (Camera.main.ScreenToWorldPoint (Input.mousePosition));

		if (input.RotateL()) {
			RotateLeft ();
		} else if (input.RotateR()) {
			RotateRight ();
		}
		
		if (input.Dampen ()) 
			Dampen ();
		
		if (input.Thrust ()) 
			ThrustEngines ();
		
		if (input.Shoot ())
			FireWeapons ();


	}
}

