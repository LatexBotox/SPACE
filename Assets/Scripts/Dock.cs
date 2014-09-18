using UnityEngine;
using System.Collections;

public class Dock : MonoBehaviour
{

	void OnTriggerEnter2D(Collider2D other) {
		print ("lol trigger");
		other.gameObject.SetActive(false);
	}
}

