using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour
{
	public GameObject parent;
	public GameObject spawnpoint;

	protected float cooldown;
	protected float t_fired = 0f;

	public abstract void Fire();

	private void Rotate() {
		Vector2 relPos = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
		float a = Mathf.Atan2 (relPos.x*-1, relPos.y) * Mathf.Rad2Deg;
		Quaternion rotation = transform.rotation;
		rotation.eulerAngles = new Vector3(0,0,a);
		
		transform.rotation = rotation;
	}
	
	void FixedUpdate () {
		Rotate ();
	}
}

