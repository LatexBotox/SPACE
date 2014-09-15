using UnityEngine;
using System.Collections;

public class ShipController : Destructables {
	Vector2 relPos;
	public float acceleration = 100f;

	public GameObject cannon;

	public Engine[] engines;

	private Vector2 velocity;

	float thrust = 0;

	int a = 0;
	int rot;
	ParticleSystem particles;
	// Use this for initialization
	void Start () {
		health = maxHealth = 50;
		particles = GetComponentInChildren<ParticleSystem>();
	}

	void StartEngines() {
		foreach (Engine e in engines)
			e.Begin ();
	}
	
	void StopEngines() {
		foreach (Engine e in engines)
			e.End ();
	}

//
//	int nfmod(int a,int b)
//	{
//		return (Mathf.Abs(a * b) + a) % b;
//	}
//
//	int fixangle(int a)
//	{
//		return nfmod (a + 180, 360) - 180;
//	}

	// Update is called once per frame
	void FixedUpdate () {
//		Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//		Vector2 shipPos = rigidbody2D.position;
//
//		relPos = (mouse - shipPos);
//		relPos.Normalize ();
//
//		rot = fixangle(Mathf.RoundToInt (rigidbody2D.rotation));
//
//		a = Mathf.RoundToInt(rot + Mathf.Atan2 (relPos.x, relPos.y) * Mathf.Rad2Deg);
//		a += (a>180) ? -360 : (a<-180) ? 360 : 0;

		if (thrust != Input.GetAxis ("Thrust")) {
			thrust = Input.GetAxis ("Thrust");
			if(thrust>0) {
				StartEngines ();
			} else {
				StopEngines ();
			}
		}

		Camera.main.orthographicSize = Mathf.Lerp (45, 55, rigidbody2D.velocity.magnitude/150);

//		if (a > 5) {
//			rigidbody2D.AddTorque (-acceleration);
//		} else if (a < -5) {
//			rigidbody2D.AddTorque(acceleration);
//		}


		if (Input.GetAxis ("Rotation")<0) {
			rigidbody2D.AddTorque (acceleration);
		} else if (Input.GetAxis ("Rotation")>0) {
			rigidbody2D.AddTorque(-acceleration);
		}

//		if (Input.GetKey (KeyCode.LeftShift)) {
//			rigidbody2D.AddForce (rigidbody2D.velocity*-6);
//		} else if (Input.GetKey (KeyCode.Space)) {
//			rigidbody2D.AddForce (relPos * acceleration);
//			particles.enableEmission = true;
//		} else {
//			particles.enableEmission = false;
//		}

		if (thrust < 0) 
			rigidbody2D.AddForce (rigidbody2D.velocity*-6);


		if (thrust > 0) 
			foreach (Engine e in engines)
				e.Thrust();

		velocity = rigidbody2D.velocity;
	}


	void OnCollisionEnter2D(Collision2D col) {
		double d = (rigidbody2D.velocity-velocity).sqrMagnitude*0.01;

		Debug.Log (d);
		Damage (d);
	}
}
