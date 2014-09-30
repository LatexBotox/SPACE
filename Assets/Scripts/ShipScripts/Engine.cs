using UnityEngine;
using System.Collections;

public class Engine : ShipComponent {

	ParticleSystem[] ps;
	Light[] glow;
	GameObject parent;

	public float thrustMult;
	public float maxSpeed;

	bool on;

	void Start() {
		parent = gameObject.transform.parent.gameObject;
		ps = GetComponentsInChildren<ParticleSystem>();
		glow = GetComponentsInChildren<Light>();
	}

	public void Init() {
		parent = gameObject.transform.parent.gameObject;
		ps = GetComponentsInChildren<ParticleSystem>();
		glow = GetComponentsInChildren<Light>();
	}

	void FixedUpdate() {
		if (!on) {
			foreach (ParticleSystem p in ps)
				p.enableEmission = false;
			foreach (Light l in glow)
				l.enabled = false;
		}
		on = false;
	}

	public void Thrust () {
		parent.rigidbody2D.AddForce (parent.transform.up.normalized * thrustMult);
		parent.rigidbody2D.AddForce (-parent.rigidbody2D.velocity.normalized * 
		                             Mathf.Lerp (0, thrustMult, (parent.rigidbody2D.velocity.magnitude)*20  / maxSpeed - 19 ));
		on = true;
		foreach (ParticleSystem p in ps)
			p.enableEmission = true;
		foreach (Light l in glow)
			l.enabled = true;
	}


}
