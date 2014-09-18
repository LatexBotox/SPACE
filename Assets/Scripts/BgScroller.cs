using UnityEngine;
using System.Collections;

public class BgScroller : MonoBehaviour {

	public Rigidbody2D ship;
	public float scroll_speed;

	private Vector2 s_offset;

	void Start () {
		s_offset = renderer.sharedMaterial.GetTextureOffset("_MainTex");
	}

	void Update () {


		float scroll_x = Mathf.Repeat(Camera.main.transform.position.x * scroll_speed, 1);
		float scroll_y = Mathf.Repeat(Camera.main.transform.position.y * scroll_speed, 1);
		renderer.sharedMaterial.SetTextureOffset("_MainTex", new Vector2(scroll_x, scroll_y));

		
		Vector3 pos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
		transform.position = pos;
	}

	void OnDisable() {
		renderer.sharedMaterial.SetTextureOffset("_MainTex", s_offset);
	}
}
