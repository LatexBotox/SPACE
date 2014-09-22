using UnityEngine;
using System.Collections;

public class PathFinder : MonoBehaviour {
	int layerMask;
	public int preferredDistance = 40;
	public int maxIterations = 5;
	public int cRadius = 7;

	void Start () {
		layerMask = 1 << 8 | 1 << 9 | 1 << 10;
	}

	public virtual Vector2 FindPath (Ship self, Ship target) {
		Vector2 pos = self.rigidbody2D.position;
		Vector2 tar = target.rigidbody2D.position;
		Vector2 vel = self.rigidbody2D.velocity;


		//Ignore Self when raycasting.
		int oldLayer = self.gameObject.layer;
		self.gameObject.layer = 2;

		Vector2 heading = Plot (pos+vel*0.5f, tar, target);
		Debug.DrawLine(pos, heading);
		Vector2 relPos = (pos - tar);
		if (heading == tar && Mathf.Abs(relPos.magnitude) < preferredDistance) {
			heading = ChooseHeading (Plot(pos, pos+new Vector2(-relPos.y,relPos.x).normalized*40),
			                         Plot(pos, pos+new Vector2(relPos.y,-relPos.x).normalized*40),
			                         self);
		}

		//Stop Ignoring yourself.
		self.gameObject.layer = oldLayer;

		return heading;
	}
	
	Vector2 ChooseHeading (Vector2 h1, Vector2 h2, Ship self) {

		if (Vector2.Dot (h1 - self.rigidbody2D.position, self.rigidbody2D.velocity) >
			Vector2.Dot (h2 - self.rigidbody2D.position, self.rigidbody2D.velocity)) {
			return h1;
		} else {
			return h2;
		}

	}

	Vector2 Plot (Vector2 from, Vector2 to, Ship target = null) {
		Vector2 relPos = to-from;
		
		RaycastHit2D firstRay = Physics2D.CircleCast (from, cRadius, relPos, Mathf.Infinity, layerMask);
		
		if (!firstRay) {
			return to;
		}

		if (target && firstRay.transform.gameObject.Equals(target.gameObject)) {
			return to;
		}

		Vector2 silly = new Vector2 (relPos.y, -relPos.x);
		silly.Normalize ();
		
		Vector2 newPos = firstRay.centroid;
		float distance = relPos.magnitude / 2;
		
		for (int i = 1; i < maxIterations; i++) {
			newPos = newPos+silly*Mathf.Pow (-1, i)*i*7;
			RaycastHit2D iterRay = Physics2D.CircleCast (from, cRadius, newPos-from, distance, layerMask);
			
			if (!iterRay.collider || target && iterRay.transform.gameObject.Equals (target.gameObject)) {
				return from+(newPos-from).normalized*distance;
			}
		}
		
		return from + firstRay.normal*30;
	}
	
}
