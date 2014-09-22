using UnityEngine;
using System.Collections;

public class AsteroidLauncher : Weapon
{
		
	protected override void Start() {
		base.Start ();
		cooldown = 1.5f;
	}

	public override void Fire ()
	{
		print (t_fired + " " + Time.time);
		if(Time.time > t_fired + cooldown) {
			print ("Fire!");
			t_fired = Time.time;
			GameObject asteroid = Instantiate( Resources.Load("asteroid_m"),
            	                           spawnpoint.transform.position + spawnpoint.transform.up * 20.0f, 
                                         spawnpoint.transform.rotation) as GameObject;

			asteroid.rigidbody2D.AddForce(spawnpoint.transform.up*500000.0f);
			
		}
	}	
}

