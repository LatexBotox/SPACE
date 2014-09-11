using UnityEngine;
using System.Collections;

public abstract class Engine : MonoBehaviour {

	public ParticleSystem ps;
	public Light glow;
	public GameObject parent;

	protected float thrustMult;
	protected float maxSpeed;

	// Update is called once per frame
	public void Thrust () {
		parent.rigidbody2D.AddForce (parent.transform.up.normalized * thrustMult);
		parent.rigidbody2D.AddForce (-parent.rigidbody2D.velocity.normalized * 
		                             Mathf.Lerp (0, thrustMult, (parent.rigidbody2D.velocity.magnitude)*20  / maxSpeed - 19 ));
		print (parent.rigidbody2D.velocity.magnitude);
	}

	public void Start () {
		ps.enableEmission = true;
		glow.enabled = true;
	}

	public void Stop () {
		ps.enableEmission = false;
		glow.enabled = false;
	}
}
