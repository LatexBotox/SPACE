using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;


[Serializable]
public class AsteroidData {
	public bool extra = false;
	public int index, size;
	public float health;
	public Asteroid.Mineral mineral;
	public float posx, posy;
}

[Serializable]
public class ChunkData {
	public int chunkx, chunky;
	public AsteroidData[] asteroidData;
}

public class Chunk : MonoBehaviour {

	public List<AsteroidData> flaggedAsteroids;
	public List<Asteroid> asteroids;
	int chunkSize = 128;

	public int chunkx, chunky;

	public AsteroidGenerator gen;
	bool alive = false;

	public int chunkSeed;

	// Use this for initialization
	void Start () {
		if (flaggedAsteroids == null)
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
					gen.transform.position = new Vector3(ad.posx,ad.posy,gen.transform.position.z);
					Asteroid clone = gen.GenerateAsteroid(ad.mineral, ad.size);
					clone.Health = ad.health;
					clone.gen = gen;
					clone.chunk = this;
					asteroids.Add (clone);
				} else if (ad.health <= 0) {
					Destroy (asteroids[ad.index].gameObject,0f);
					asteroids[ad.index] = null;
				} else if (ad.index != -1) {
					Asteroid asteroid = asteroids[ad.index];
					asteroid.rigidbody2D.position = new Vector2(ad.posx, ad.posy);
					asteroid.Health = ad.health;
				}
			}

			foreach (Asteroid a in asteroids)
				if (a!=null)
					a.gameObject.SetActive(true);

		}
		CancelInvoke();
		Invoke ("DestroyAsteroids", 5);
	}

	void DestroyAsteroids () {
		Asteroid asteroid;

		foreach (AsteroidData ad in flaggedAsteroids) {

			asteroid = asteroids[ad.index];
			if (asteroid == null) {
				ad.health = 0;
				if (ad.extra) {
					ad.index = -1;
				}
			} else {
				ad.health = asteroid.Health;
				ad.posx = asteroid.rigidbody2D.position.x;
				ad.posy = asteroid.rigidbody2D.position.y;
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
		if (asteroids.IndexOf(asteroid)!=-1)	
			asteroids[asteroids.IndexOf(asteroid)] = null;
	}

	public void AddAsteroid(Asteroid asteroid) {
		asteroid.flagged = true;
		asteroids.Add (asteroid);
		flaggedAsteroids[FlagAsteroid (asteroid)].extra = true;
		asteroid.gameObject.SetActive(true);
	}

	public int FlagAsteroid(Asteroid asteroid) {
		AsteroidData ad = new AsteroidData();
		if (!asteroid)
			return -1;

		ad.health = asteroid.Health;
		ad.mineral = asteroid.mineral;
		ad.posx = asteroid.rigidbody2D.position.x;
		ad.posy = asteroid.rigidbody2D.position.y;
		ad.index = asteroids.IndexOf (asteroid);
		ad.size = asteroid.sizeClass;
		flaggedAsteroids.Add (ad);

		return flaggedAsteroids.IndexOf (ad);
	}

	public ChunkData GetChunkData() {
		if (flaggedAsteroids.Count > 0) {
			Asteroid asteroid;

			foreach (AsteroidData ad in flaggedAsteroids) {
				if (ad.index < asteroids.Count) {
					asteroid = asteroids[ad.index];
				} else {
					asteroid = null;
				}
				if (asteroid == null) {
					ad.health = 0;
					if (ad.extra)
						ad.index = -1;
				} else {
					ad.health = asteroid.Health;
					ad.posx = asteroid.rigidbody2D.position.x;
					ad.posy = asteroid.rigidbody2D.position.y;
				}
			}
			
			flaggedAsteroids.RemoveAll ((AsteroidData obj) => obj.index == -1);

			ChunkData cd = new ChunkData();
			cd.asteroidData = flaggedAsteroids.ToArray();
			cd.chunkx = chunkx;
			cd.chunky = chunky;
			return cd;
		}
		return null;
	}
}
