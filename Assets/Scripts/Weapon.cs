using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour
{
	protected float cooldown = 0f;
	protected float t_fired = 0f;
	
	public abstract void Fire(GameObject spawnpoint);

	public abstract void Reset();
}

