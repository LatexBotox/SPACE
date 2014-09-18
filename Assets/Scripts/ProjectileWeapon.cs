using UnityEngine;
using System.Collections;

public class ProjectileWeapon : Weapon {
	public Projectile projectile;

	void Start() {
		cooldown = 0.3f;
	}

	public override void Fire() {
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
