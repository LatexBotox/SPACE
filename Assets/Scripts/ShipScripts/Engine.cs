using UnityEngine;
using System.Collections;

public class Engine : MonoBehaviour {

	public ParticleSystem ps;
	public Light glow;
	GameObject parent;

	public float thrustMult;
	public float maxSpeed;

	bool on;

	void Start() {
		parent = gameObject.transform.parent.gameObject;
	}

	void FixedUpdate() {
		if (!on) 
			ps.enableEmission = glow.enabled = false;
		on = false;
	}

	public void Thrust () {
		parent.rigidbody2D.AddForce (parent.transform.up.normalized * thrustMult);
		parent.rigidbody2D.AddForce (-parent.rigidbody2D.velocity.normalized * 
		                             Mathf.Lerp (0, thrustMult, (parent.rigidbody2D.velocity.magnitude)*20  / maxSpeed - 19 ));
		ps.enableEmission = glow.enabled = on = true;
	}


}
