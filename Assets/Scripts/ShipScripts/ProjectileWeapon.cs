using UnityEngine;
using System.Collections;

public class ProjectileWeapon : Weapon {
	public Projectile projectile;
	public float setCooldown;

	protected override void Start() {
		base.Start ();
		cooldown = setCooldown;
	}

	public override void Fire() {
//		print ("Time: " + Time.time + ", Last Fired: " + t_fired + ", Cooldown: " + cooldown);

		if(Time.time > t_fired + cooldown) {
			t_fired = Time.time;

			Projectile projectileClone = Instantiate( projectile,
			                                         spawnpoint.transform.position, 
			                                         spawnpoint.transform.rotation) as Projectile;

			Physics2D.IgnoreCollision(projectileClone.collider2D, parent.collider2D);

			projectileClone.launchedLayer = parent.layer;
		}
	}
}
