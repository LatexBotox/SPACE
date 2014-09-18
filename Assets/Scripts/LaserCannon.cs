using UnityEngine;
using System.Collections;

public class LaserCannon : Weapon
{


	void Start() {
		print ("start: " + t_fired);

		cooldown = 0.3f;
		t_fired = 0.0f;
	}

	public override void Reset() {
		t_fired = 0.0f;
		cooldown = 0.3f;
	}
	
	public override void Fire(GameObject spawnpoint) {
		if(Time.time > t_fired + cooldown) {
			t_fired = Time.time;
			Instantiate( Resources.Load("laser"),
               	   spawnpoint.transform.position, 
                 	 spawnpoint.transform.rotation);
		}
	}
}

