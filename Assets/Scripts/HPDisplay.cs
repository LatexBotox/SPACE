using UnityEngine;
using System.Collections;

public class HPDisplay : MonoBehaviour {

	public Destructables ship;
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
//		if (ship)
		guiText.text = Mathf.Ceil ((float)ship.Health).ToString();
	}
}
