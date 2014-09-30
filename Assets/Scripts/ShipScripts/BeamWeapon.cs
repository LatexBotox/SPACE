using UnityEngine;
using System.Collections;

public class BeamWeapon : Weapon
{
	public float range;
	public float width;
	public float damage;

	public ParticleSystem[] beams;
	public LayerMask mask;
	public ParticleSystem impact;

	bool firing;


	protected override void Start ()
	{
		base.Start();
		beams = GetComponentsInChildren<ParticleSystem>();
		mask = 1<<10|(parent.gameObject.layer==8?0:1)<<8|(parent.gameObject.layer==9?0:1)<<9;
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
			if (impact) {
				impact.transform.position = hit.point+hit.normal*1.5f;
				impact.transform.rotation = Quaternion.LookRotation(hit.normal, impact.transform.forward);
				impact.Emit (100);
			}
		}
	}
}

