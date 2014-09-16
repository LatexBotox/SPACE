using UnityEngine;
using System.Collections;

public class CannonScript : MonoBehaviour {
	public float cooldown = 0.3f;
	public float curCooldown = 0f;

	public Rigidbody2D projectile;
	public GameObject spawnpoint;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 relPos = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;

		
		float a = Mathf.Atan2 (relPos.x*-1, relPos.y) * Mathf.Rad2Deg;


		Quaternion rotation = transform.rotation;
		rotation.eulerAngles = new Vector3(0,0,a);

		transform.rotation = rotation;

		curCooldown -= Time.deltaTime;
		if (Input.GetKey(KeyCode.Mouse0) && curCooldown <= 0) {
			Rigidbody2D projectileClone = (Rigidbody2D)Instantiate (projectile,spawnpoint.transform.position, spawnpoint.transform.rotation);
			curCooldown = cooldown;
		}
	}
}
