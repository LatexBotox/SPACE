using UnityEngine;
using System.Collections;

public class ProjectileWeapon : Weapon {
	public Projectile projectile;
	public float setCooldown;

	protected override void Start() {
		base.Start ();
		cooldown = setCooldown;
		rotates = true;
	}

	public override void Fire() {
//		print ("Time: " + Time.time + ", Last Fired: " + t_fired + ", Cooldown: " + cooldown);

		if(Time.time > t_fired + cooldown) {
			t_fired = Time.time;

			Projectile projectileClone = Instantiate( projectile,
			                                         spawnpoint.transform.position, 
			                                         spawnpoint.transform.rotation) as Projectile;

			foreach (Collider2D c in GetComponentsInParent<Collider2D>())
				Physics2D.IgnoreCollision(projectileClone.collider2D, c);

			projectileClone.launchedLayer = parent.layer;
		}
	}
}
