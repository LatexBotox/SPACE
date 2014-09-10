using UnityEngine;
using System.Collections;

public class Pickup1 : MonoBehaviour {




	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void Pickup() {
		print ("lol got picked");
		Destroy (gameObject, 0f);
	}
  
	void OnCollisionEnter2D(Collision2D col) {
		print ("lol collision");
		if (col.gameObject.tag == "Player") {
			Pickup();
		}
	}

}
