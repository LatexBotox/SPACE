using UnityEngine;
using System.Collections;

public class ScrollingParticles : MonoBehaviour {
	public GameObject player;

	void Start () {
		if (!player) {
			enabled = false;
			return;
		}
		particleSystem.transform.position = player.transform.position;
		particleSystem.transform.rotation = Quaternion.AngleAxis (Mathf.Atan2(player.rigidbody2D.velocity.y,
		                                                                      player.rigidbody2D.velocity.x)*Mathf.Rad2Deg-90,
		                                                          						transform.forward);
		particleSystem.enableEmission = true;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!player)
			DestroyImmediate (gameObject);
		particleSystem.transform.position = player.transform.position;
		particleSystem.transform.rotation = Quaternion.AngleAxis (Mathf.Atan2(player.rigidbody2D.velocity.y,
		                                                                      player.rigidbody2D.velocity.x)*Mathf.Rad2Deg-90,
		                                                          						transform.forward);
		particleSystem.emissionRate = (player.rigidbody2D.velocity.magnitude-40) * 100;
	}
	
}
