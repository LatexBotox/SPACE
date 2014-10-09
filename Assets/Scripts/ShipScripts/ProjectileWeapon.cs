using UnityEngine;
using System.Collections;

public class ProjectileWeapon : Weapon {
	public Projectile projectile;
	public float setCooldown;

	protected override void Start() {
		
		cooldown = setCooldown;
		base.Start ();
	}

	public override void Fire() {
		//print ("Time: " + Time.time + ", Last Fired: " + t_fired + ", Cooldown: " + cooldown);

		if(Time.time > t_fired + cooldown) {
			t_fired = Time.time;
			//print (cooldown);

			Projectile projectileClone = Instantiate( projectile,
			                                         spawnpoint.transform.position, 
			                                         spawnpoint.transform.rotation) as Projectile;

			foreach (Collider2D c in GetComponentsInParent<Collider2D>())
				Physics2D.IgnoreCollision(projectileClone.collider2D, c);

			foreach( Collider2D c in parent.GetComponents<Collider2D>())
				Physics2D.IgnoreCollision(projectileClone.collider2D, c);

			projectileClone.launchedLayer = parent.layer;
		}
	}
}
