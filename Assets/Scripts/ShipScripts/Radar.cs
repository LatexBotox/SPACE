using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Radar : MonoBehaviour {

	public GameObject parent;
	public float range;
	public IndicatorArrow arrow;
	Dictionary<int, IndicatorArrow> arrows;

	void Start() {
		arrows = new Dictionary<int,IndicatorArrow> ();
	}

	void FixedUpdate() {
		if (!parent) {
			DestroyImmediate (gameObject);
			return;
		}
		transform.position = parent.transform.position;
	}


	void OnTriggerEnter2D(Collider2D other) {
		IndicatorArrow arrowClone = Instantiate (arrow, new Vector3(parent.transform.position.x,parent.transform.position.y,arrow.transform.position.z),
		                                         arrow.transform.rotation) as IndicatorArrow;
		arrowClone.transform.parent = parent.transform;
		arrowClone.target = other.gameObject;
		arrowClone.fromPos = parent;
//		arrowClone.rotate();
		arrowClone.maxRange = range;

		arrows[other.GetInstanceID ()] = arrowClone;
	}

	void OnTriggerExit2D(Collider2D other) {
		Destroy (arrows[other.GetInstanceID()].gameObject, 0);
		arrows.Remove (other.GetInstanceID ());
	}
}
