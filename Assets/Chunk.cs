using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[SerializeField]
public class AsteroidData {
	public bool extra = false;
	public int index, size;
	public float health;
	public Asteroid.Mineral mineral;
	public Vector2 pos;
}

public class Chunk : MonoBehaviour {

	List<AsteroidData> flaggedAsteroids;
	public List<Asteroid> asteroids;
	int chunkSize = 128;

	public int chunkx, chunky;

	public AsteroidGenerator gen;
	bool alive = false;

	public int chunkSeed;

	// Use this for initialization
	void Start () {
		flaggedAsteroids = new List<AsteroidData>();
		asteroids = new List<Asteroid>();
		chunkx = Mathf.FloorToInt (transform.position.x/128);
		chunky = Mathf.FloorToInt (transform.position.y/128);
		gen = GameObject.FindGameObjectWithTag("AsteroidGenerator").GetComponent<AsteroidGenerator>();;

		chunkSeed = gen.levelSeed+chunkx+chunky*chunkSize;

		if (!gen)
			enabled = false;
	}

	// Update is called once per frame

	public void Generate () {
		if (!gen)
			return;

		if (!alive) {
			asteroids = new List<Asteroid>();
			gen.GenerateChunk(this);
			alive = true;

			foreach (AsteroidData ad in flaggedAsteroids) {
				if (ad.index > asteroids.Count-1) {
					gen.transform.position = ad.pos;
					Asteroid clone = gen.GenerateAsteroid(ad.mineral, ad.size);
					clone.Health = ad.health;
					clone.gen = gen;
					clone.chunk = this;
				} else if (ad.health == 0 && asteroids[ad.index] != null) {
					Destroy (asteroids[ad.index].gameObject,0f);
					asteroids[ad.index] = null;
				} else {
					print ("Restored asteroid at index: " +ad.index);
					Asteroid asteroid = asteroids[ad.index];
					asteroid.rigidbody2D.position = ad.pos;
					asteroid.Health = ad.health;
				}
			}

		}
		CancelInvoke();
		Invoke ("DestroyAsteroids", 5);
	}

	void DestroyAsteroids () {
		Asteroid asteroid;

		foreach (AsteroidData ad in flaggedAsteroids) {

			print ("Getting data from asteroid at: " + ad.index);
			asteroid = asteroids[ad.index];
			if (asteroid == null) {
				ad.health = 0;
				if (ad.extra)
					ad.index = -1;
			} else {
				ad.health = asteroid.Health;
				ad.pos = asteroid.transform.position;
			}
		}

		flaggedAsteroids.RemoveAll ((AsteroidData obj) => obj.index == -1);

		foreach (Asteroid a in asteroids) {
			if (a!= null)
				Destroy (a.gameObject,0f);
		}

		alive = false;
	}

	public void RemoveAsteroid(Asteroid asteroid) {
		asteroids[asteroids.IndexOf(asteroid)] = null;
	}

	public void AddAsteroid(Asteroid asteroid) {
		asteroid.flagged = true;
		asteroids.Add (asteroid);
		flaggedAsteroids[FlagAsteroid (asteroid)].extra = true;
	}

	public int FlagAsteroid(Asteroid asteroid) {
		AsteroidData ad = new AsteroidData();
		if (!asteroid)
			return -1;

		ad.health = asteroid.Health;
		ad.mineral = asteroid.mineral;
		ad.pos = asteroid.rigidbody2D.position;
		ad.index = asteroids.IndexOf (asteroid);
		ad.size = asteroid.sizeClass;
		flaggedAsteroids.Add (ad);

		return flaggedAsteroids.IndexOf (ad);
	}
}
