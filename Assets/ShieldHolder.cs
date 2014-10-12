using UnityEngine;
using System.Collections;

public class ShieldHolder : MonoBehaviour {
	public Shield shieldprefab;
	Shield shield;

	public Shield BuildShield(float radius, PolygonCollider2D pcol) {
		shield = Instantiate (shieldprefab, transform.position, transform.rotation) as Shield;
		shield.transform.parent = transform;
		shield.radius = radius;
		shield.shcollider = (CircleCollider2D)collider2D;//transform.parent.gameObject.GetComponent<CircleCollider2D>();
		shield.pcollider = pcol;

		return shield;
	}

	public void OnCollisionEnter2D(Collision2D col) {
		shield.Impact (col.contacts[0].point-(Vector2)transform.position);
	}
}
