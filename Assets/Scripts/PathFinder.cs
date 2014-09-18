using UnityEngine;
using System.Collections;

public class PathFinder : MonoBehaviour {

	public Transform pos;
	public Transform tar;

	int update = 0;

	int layerMask;

	void Start () {
		layerMask = 1 << 8 | 1 << 9 | 1 << 10;

		print (layerMask);
	}

	void FixedUpdate () {
		int oldLayer = pos.gameObject.layer;
		pos.gameObject.layer = 2;
		Vector2 heading = Plot (pos.position, tar.position);
		Debug.DrawLine(pos.position, heading);
		Vector2 relPos = (pos.position - tar.position);
		if (heading == (Vector2)tar.position && Mathf.Abs(relPos.magnitude) < 100) {
			Vector2 newHeading = (Vector2)pos.position+new Vector2(relPos.y,
			                                 											-relPos.x).normalized*20;

			newHeading = Plot (pos.position, newHeading);
			Debug.DrawLine (pos.position, newHeading);
//			heading = Plot (pos.position, new Vector2(-relPos.y+tar.position.x,
//			                                          relPos.x+tar.position.y).normalized*10);

			newHeading = (Vector2)pos.position+new Vector2(-relPos.y,
			                         												relPos.x).normalized*20;
			newHeading = Plot (pos.position, newHeading);
			Debug.DrawLine (pos.position, newHeading);
		}
		pos.gameObject.layer = oldLayer;
		update = 0;
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
