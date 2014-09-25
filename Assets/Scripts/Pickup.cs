using UnityEngine;
using System.Collections;

public abstract class Pickup : MonoBehaviour {

	protected virtual void Pick() {
		Destroy (gameObject, 0f);
	}
  
	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Player") {
			Pick();
		}
	}
}
