using UnityEngine;
using System.Collections;

public class ScrollingParticles : MonoBehaviour {

	
	// Update is called once per frame
	void FixedUpdate () {
		if (PlayerShip.instance == null) {
			particleSystem.enableEmission = false;
			return;
		}
		GameObject player = PlayerShip.instance.gameObject;

		particleSystem.enableEmission = true;
		particleSystem.transform.position = player.transform.position;
		particleSystem.transform.rotation = Quaternion.AngleAxis (Mathf.Atan2(player.rigidbody2D.velocity.y,
		                                                                      player.rigidbody2D.velocity.x)*Mathf.Rad2Deg-90,
		                                                          						transform.forward);
		particleSystem.emissionRate = (player.rigidbody2D.velocity.magnitude-40) * 100;
	}
	
}
