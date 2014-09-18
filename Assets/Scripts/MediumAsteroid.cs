using UnityEngine;
using System.Collections;

public class MediumAsteroid : Destructables
{
	public GameObject smallAsteroid;
	public ParticleSystem dieExplo;

	// Use this for initialization
	void Start ()	
	{
		Col_dmg_scaler = 0.5f;
		health = 1000f;
		maxHealth = 1000f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		oldVelocity = rigidbody2D.velocity;
	}

	private void SpawnSmallAsteroid(uint nr_asteroids) {
		for(int i = 0; i < nr_asteroids; +++i) {
			Vector2 r_dir = Random.insideUnitCircle;
			GameObject clone = Instantiate(smallAsteroid, ((Vector3)(((Vector2)transform.position) + r_dir*10f)) + new Vector3(0,0,smallAsteroid.transform.position.z)
			                               , transform.rotation) as GameObject;
			Vector2 vel = rigidbody2D.velocity;
			Vector2 n = new Vector2(vel.y, -vel.x) * (0.5f - Random.value);
			float rand_dir_scale = 5f;
			Vector2 r = new Vector2((0.5f - Random.value), (0.5f - Random.value)) * rand_dir_scale; 
			clone.rigidbody2D.velocity = vel + n.normalized * 0.5f * vel.magnitude + r;
		}
	}

	public override void Die()
	{
		//dead = true;
		//SpawnSmallAsteroid();
		ParticleSystem explosion = Instantiate(dieExplo, ((Vector2)transform.position), transform.rotation) as ParticleSystem;
		Destroy (explosion, 2.0f);
		SpawnSmallAsteroid(3);
		Destroy(gameObject);
	}
}

