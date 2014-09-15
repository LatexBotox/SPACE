using UnityEngine;
using System.Collections;

public class Engine : MonoBehaviour {

	public ParticleSystem ps;
	public Light glow;
	public GameObject parent;

	public float thrustMult;
	public float maxSpeed;
	
	public void Thrust () {
		parent.rigidbody2D.AddForce (parent.transform.up.normalized * thrustMult);
		parent.rigidbody2D.AddForce (-parent.rigidbody2D.velocity.normalized * 
		                             Mathf.Lerp (0, thrustMult, (parent.rigidbody2D.velocity.magnitude)*20  / maxSpeed - 19 ));
	}

	public void Begin () {
		ps.enableEmission = true;
		glow.enabled = true;
	}

	public void End () {
		ps.enableEmission = false;
		glow.enabled = false;
	}
}
