using UnityEngine;
using System.Collections;

public class Cockpit : MonoBehaviour {

	public float range;
	public GameObject parent;
	public Radar radar;
	
	void Start () {
		radar = Instantiate (radar, parent.transform.position, radar.transform.rotation) as Radar;

		Physics2D.IgnoreCollision (radar.collider2D, parent.collider2D);
		((CircleCollider2D)radar.collider2D).radius = range;

		radar.range = range;
		radar.parent = parent;
	}
}
