using UnityEngine;
using System.Collections;

public class Sucker : MonoBehaviour
{
	float G = 0.01f;

	void Start() {
	}


	void Update ()
	{

		Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 100f, 1<<10 | 1<<9);

		foreach(Collider2D c in cols) {
			Vector2 dir = transform.position - c.transform.position;
			Vector2 fdir = dir.normalized;
			float r = dir.magnitude;

			float force = ((G*c.rigidbody2D.mass*transform.parent.rigidbody2D.mass)/r*r)*Time.deltaTime;
			c.rigidbody2D.AddForce(fdir*force);
		}
	}
}

