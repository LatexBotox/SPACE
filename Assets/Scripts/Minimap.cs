using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour {
	public GameObject follow;
	// Use this for initialization
	void Start () {
		camera.pixelRect = new Rect (50, 50, 200, 200);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		pos.x = follow.transform.position.x;
		pos.y = follow.transform.position.y;

		transform.position = pos;
	}
}
