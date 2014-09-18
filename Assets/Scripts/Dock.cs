using UnityEngine;
using System.Collections;

public class Dock : MonoBehaviour
{

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			other.gameObject.SetActive (false);
			Application.LoadLevel(0);

		}
	}
}

