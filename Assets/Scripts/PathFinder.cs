using UnityEngine;
using System.Collections;

public class PathFinder : MonoBehaviour {

	public ShipController ship;
	public Transform pos;
	public Transform tar;

	float preferredDistance = 60;

	int update = 0;

	int layerMask;

	void Start () {
		layerMask = 1 << 8 | 1 << 9 | 1 << 10;

		print (layerMask);
	}

	void FixedUpdate () {
		if (!tar) 
			enabled = false;
		if (!ship) {
			DestroyImmediate (gameObject);
			return;
		}

		int oldLayer = pos.gameObject.layer;
		pos.gameObject.layer = 2;
		Vector2 heading = Plot ((Vector2)pos.position+ship.rigidbody2D.velocity*0.5f, tar.position);
		Debug.DrawLine(pos.position, heading);
		Vector2 relPos = (pos.position - tar.position);
		if (heading == (Vector2)tar.position && Mathf.Abs(relPos.magnitude) < preferredDistance) {
//			Vector2 newHeading = (Vector2)pos.position+new Vector2(relPos.y,
//			                                 											-relPos.x).normalized*35;
//
//			newHeading = Plot (pos.position, newHeading);
//			Debug.DrawLine (pos.position, newHeading);
//
//			newHeading = (Vector2)pos.position+new Vector2(-relPos.y,
//			                         												relPos.x).normalized*35;
//			newHeading = Plot (pos.position, newHeading);
//			Debug.DrawLine (pos.position, newHeading);

			heading = ChooseHeading ((Vector2)pos.position+new Vector2(-relPos.y,relPos.x).normalized*40+ship.rigidbody2D.velocity*0.5f,
			                         (Vector2)pos.position+new Vector2(relPos.y,-relPos.x).normalized*40+ship.rigidbody2D.velocity*0.5f);
		}
		ship.moveTowards(heading);

		pos.gameObject.layer = oldLayer;
		update = 0;
	}
	
	Vector2 ChooseHeading (Vector2 h1, Vector2 h2) {

		if (Vector2.Dot (h1 - ship.rigidbody2D.position, ship.rigidbody2D.velocity) >
			Vector2.Dot (h2 - ship.rigidbody2D.position, ship.rigidbody2D.velocity)) {
			return h1;
		} else {
			return h2;
		}

	}

	Vector2 Plot (Vector2 from, Vector2 to) {
		Vector2 relPos = to-from;
		
		RaycastHit2D firstRay = Physics2D.CircleCast (from, 6, relPos, Mathf.Infinity, layerMask);
		
		if (!firstRay) {
			return to;
		}
		
		print (firstRay.transform.name + " on layer " + firstRay.transform.gameObject.layer);
		if (firstRay.transform.gameObject.Equals(tar.gameObject)) {
			return to;
		}
		
		Vector2 colPoint = firstRay.centroid;
		Vector2 silly = new Vector2 (relPos.y, -relPos.x);
		silly.Normalize ();
		
		Vector2 newPos = firstRay.centroid;
		float distance = relPos.magnitude / 2;
		
		for (int i = 1; i < 13; i++) {
			newPos = newPos+silly*Mathf.Pow (-1, i)*i*7;
			RaycastHit2D iterRay = Physics2D.CircleCast (from, 6, newPos-from, distance, layerMask);
			
			if (!iterRay.collider || iterRay.transform.gameObject.Equals (tar.gameObject)) {
				return from+(newPos-from).normalized*distance;
			}
		}
		
		return from + firstRay.normal*30;
	}




//	Vector2 PlotLines (Vector2 from, Vector2 to, int depth) {
//		Vector2 relPos = to-from;
//		
//		if (depth > 15)
//			return from;
//		
//		RaycastHit2D firstRay = Physics2D.CircleCast (from, 6, relPos, Mathf.Infinity, layerMask);
//		
//		if (!firstRay) {
//			print ("Fuck it");
//			return to;
//		}
//		
//		print (firstRay.transform.name + " on layer " + firstRay.transform.gameObject.layer);
//		if (firstRay.transform.gameObject.Equals(tar.gameObject)) {
//			return to;
//		}
//		
//		Vector2 colPoint = firstRay.centroid;
//		Vector2 silly = new Vector2 (relPos.y, -relPos.x);
//		silly.Normalize ();
//		
//		Vector2 newPos;
//		float distance = relPos.magnitude / 2;
//		
//		for (int i = 1; i < 13; i++) {
//			newPos = colPoint+silly*Mathf.Pow (-1, i)*i*7;
//			RaycastHit2D iterRay = Physics2D.CircleCast (from, 6, newPos-from, distance, layerMask);
//			
//			if (!iterRay.collider || iterRay.transform.gameObject.Equals (tar.gameObject)) {
//				
//				Debug.DrawLine (from+(newPos-from).normalized*distance, 
//				                PlotLines (from+(newPos-from).normalized*distance, to, depth+1));
//				
//				
//				return from+(newPos-from).normalized*distance;
//			}
//		}
//		
//		return from;
//	}
}
