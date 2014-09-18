using UnityEngine;
using System.Collections;

public class AsteroidLauncher : Weapon
{
	void Start() {
		cooldown = 1.0f;
		t_fired = 0.0f;
	}

	public override void Reset ()
	{
		cooldown = 1.0f;
		t_fired = 0.0f;
	}

	public override void Fire (GameObject spawnpoint)
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

