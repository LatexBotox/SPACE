using UnityEngine;
using System.Collections;

public class Cockpit : ShipComponent {

	public float range;
	GameObject parent;
	public Radar radar;

	bool radarExists = false;

	public void StartRadar() {
		print ("Radar Started. Beep Boop");
		parent = gameObject.transform.parent.gameObject;
		radar = Instantiate (radar, parent.transform.position, radar.transform.rotation) as Radar;

		Physics2D.IgnoreCollision (radar.collider2D, parent.collider2D);
		((CircleCollider2D)radar.collider2D).radius = range;

		radar.range = range;
		radar.parent = parent;

		radarExists = true;
	}

	void OnDestroy() {
		if (radarExists)
			DestroyImmediate (radar);
	}
}
