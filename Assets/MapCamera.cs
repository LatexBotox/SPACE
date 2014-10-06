using UnityEngine;
using System.Collections;

public class MapCamera : MonoBehaviour {
	public float scrollArea;
	public float scrollSpeed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float mousePosX = Input.mousePosition.x;
		float mousePosY = Input.mousePosition.y;

		Vector3 trans = Vector3.zero;

		if (mousePosX < scrollArea)
			trans.x -= scrollSpeed;
		if (mousePosY < scrollArea)
			trans.y -= scrollSpeed;
		if (mousePosX > camera.pixelWidth-scrollArea)
			trans.x += scrollSpeed;
		if (mousePosY > camera.pixelHeight-scrollArea)
			trans.y += scrollSpeed;

		transform.position += trans*Time.deltaTime;
	}
}
