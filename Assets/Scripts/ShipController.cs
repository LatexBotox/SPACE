using UnityEngine;
using System.Collections;

public class ShipController : Destructables {
	Vector2 relPos;
	public float acceleration = 100f;

	public GameObject cannon;

	private Vector2 velocity;

	int a = 0;
	int rot;
	ParticleSystem particles;
	// Use this for initialization
	void Start () {
		health = maxHealth = 50;
		particles = GetComponentInChildren<ParticleSystem>();
	}
	

	int nfmod(int a,int b)
	{
		return (Mathf.Abs(a * b) + a) % b;
	}

	int fixangle(int a)
	{
		return nfmod (a + 180, 360) - 180;
	}

	// Update is called once per frame
	void Update () {
		Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 shipPos = rigidbody2D.position;

		relPos = (mouse - shipPos);
		relPos.Normalize ();

		rot = fixangle(Mathf.RoundToInt (rigidbody2D.rotation));

		a = Mathf.RoundToInt(rot + Mathf.Atan2 (relPos.x, relPos.y) * Mathf.Rad2Deg);
		a += (a>180) ? -360 : (a<-180) ? 360 : 0;



		Camera.main.orthographicSize = Mathf.Lerp (45, 55, rigidbody2D.velocity.magnitude/150);

		if (a > 5) {
			rigidbody2D.AddTorque (-acceleration);
		} else if (a < -5) {
			rigidbody2D.AddTorque(acceleration);
		}

		if (Input.GetKey (KeyCode.LeftShift)) {
			rigidbody2D.AddForce (rigidbody2D.velocity*-6);
		} else if (Input.GetKey (KeyCode.Space)) {
			rigidbody2D.AddForce (relPos * acceleration);
			particles.enableEmission = true;
		} else {
			particles.enableEmission = false;
		}
		velocity = rigidbody2D.velocity;
	}

	void OnCollisionEnter2D(Collision2D col) {
		double d = (rigidbody2D.velocity-velocity).sqrMagnitude*0.01;

		Debug.Log (d);
		Damage (d);
	}
}
