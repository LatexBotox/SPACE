using UnityEngine;
using System.Collections;

public abstract class Pickup : MonoBehaviour {

	protected virtual void Pick() {
		Destroy (gameObject, 0f);
	}
  
	void OnTriggerEnter2D(Collider2D col) {
		print ("lawl");
		if (col.gameObject.tag == "Player") {
			print ("pick me mofo");
			Pick();
		}
	}
}
