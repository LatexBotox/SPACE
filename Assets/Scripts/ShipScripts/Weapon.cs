using UnityEngine;
using System.Collections;

public abstract class Weapon : ShipComponent
{
	public GameObject parent;
	public GameObject spawnpoint;
	public bool rotates = true;

	protected float cooldown;
	protected float t_fired = 0f;

	public abstract void Fire();

	protected virtual void Start() {
		parent = gameObject.transform.parent.parent.gameObject;
	}

	public virtual void Init() {
		enabled = true;
		parent = gameObject.transform.parent.parent.gameObject;
	}

	public void Rotate(Vector2 target) {
		if (!rotates)
			return;
		Vector2 relPos = target - (Vector2)transform.position;
		transform.rotation = Quaternion.AngleAxis (Mathf.Atan2 (relPos.y, relPos.x)*Mathf.Rad2Deg-90, Vector3.forward);
	}
}

