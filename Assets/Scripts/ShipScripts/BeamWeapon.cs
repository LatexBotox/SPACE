using UnityEngine;
using System.Collections;

public class BeamWeapon : Weapon
{
	public float range;
	public float width;
	public float damage;

	public ParticleSystem[] beams;
	public LayerMask mask;

	bool firing;

	protected override void Start ()
	{
		base.Start();
		rotates = false;
		beams = GetComponentsInChildren<ParticleSystem>();

		mask = 1<<10|(parent.gameObject.layer==8?0:1)<<8|(parent.gameObject.layer==9?0:1)<<9;
		print((int)mask);
	}
	
	void FixedUpdate() {
		foreach (ParticleSystem ps in beams) {
			ps.enableEmission = firing;	
		}
		firing = false;
	}

	public override void Fire ()
	{
		foreach (ParticleSystem ps in beams) {
			ps.enableEmission = firing = true;
			ps.startRotation = Random.Range ((transform.rotation.z-5)*Mathf.Deg2Rad, (transform.rotation.z+5)*Mathf.Deg2Rad);
		}
		RaycastHit2D[] hits = Physics2D.CircleCastAll (spawnpoint.transform.position, width/2, parent.transform.up, range, mask);

		Debug.DrawRay (spawnpoint.transform.position, parent.transform.up);

		foreach (RaycastHit2D hit in hits) {
			print ("Hit object: " + hit.collider.gameObject.name);
			hit.collider.gameObject.SendMessage("Damage", damage);
		}
	}
}

