using UnityEngine;
using System.Collections;

public class StateTrigger : MonoBehaviour
{

	public ColCallback callback;

	public void SetEnabled(bool e) {
		GetComponent<Collider2D> ().enabled = e;
	}

	void OnTriggerEnter2D(Collider2D col) {
		if(col.tag != "Player")
			return;
		
		callback(this);
	}
}

