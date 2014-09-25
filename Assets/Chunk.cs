using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;


[Serializable]
public class AsteroidData {
	public int index, size; //Irregular Asteroids (spawned from save/breaking other asteroids) have a negative index.
	public bool destroyed = false;
	public Asteroid.Mineral mineral;
	public float posx, posy;
}

[Serializable]
public class ChunkData {
	public int chunkx, chunky;
	public AsteroidData[] asteroidData;
}

public class Chunk : MonoBehaviour {
	public SortedList<int, Asteroid> asteroids;
	public SortedList<int, AsteroidData> flaggedAsteroids;

	public int chunkx, chunky;
	public AsteroidGenerator gen;
	public int chunkSeed;

	int chunkSize = 128; //128 means zero stutter when generating a chunk. 

	int asteroidIndex;

	bool alive = false;



	// Use this for initialization
	void Start () {
		if (flaggedAsteroids == null)
			flaggedAsteroids = new SortedList<int, AsteroidData>();

		asteroids = new SortedList<int, Asteroid>();
		asteroidIndex = -1;

		chunkx = Mathf.FloorToInt (transform.position.x/128);
		chunky = Mathf.FloorToInt (transform.position.y/128);

		gen = GameObject.FindGameObjectWithTag("AsteroidGenerator").GetComponent<AsteroidGenerator>();;

		chunkSeed = gen.levelSeed+chunkx+chunky*chunkSize;
	}

	// Update is called once per frame

	public void Generate () {
		if (!gen)
			return; //Makes sure the chunk is properly initialized

		if (!alive) {
			alive = true;
			gen.GenerateChunk(this);

			foreach (KeyValuePair<int, AsteroidData> p in flaggedAsteroids) {
				if (p.Key < 0) {
					SpawnAsteroidFromData(p.Value);
				} else if (p.Value.destroyed) {
					Destroy (asteroids[p.Key].gameObject, 0f);
					asteroids[p.Key] = null;
				} else {
					asteroids[p.Key].rigidbody2D.position = new Vector2(p.Value.posx, p.Value.posy);
				}
			}

			foreach (KeyValuePair<int, Asteroid> p in asteroids)
				if (p.Value!=null)
					p.Value.gameObject.SetActive(true);
		}

		CancelInvoke();
		Invoke ("DestroyAsteroids", 30);
	}

	void DestroyAsteroids () {
		UpdateAsteroidData();

		foreach (KeyValuePair<int, Asteroid> p in asteroids)
			if (p.Value!= null) 
				Destroy (p.Value.gameObject,0f);

		asteroids.Clear();
		alive = false;
	}

	public void RemoveAsteroid(Asteroid asteroid) {
		if (asteroid.id < 0) {
			asteroids.Remove (asteroid.id);
			flaggedAsteroids.Remove(asteroid.id);
		} else {
			asteroids[asteroid.id] = null;
		}
	}

	public void AddAsteroid(Asteroid asteroid) {
		asteroid.id = asteroidIndex;
		asteroids.Add (asteroidIndex--, asteroid);
		asteroid.gameObject.SetActive(true);

		FlagAsteroid(asteroid);
	}

	public void FlagAsteroid(Asteroid asteroid) {
		asteroid.flagged = true;
		AsteroidData ad = new AsteroidData();
		ad.index = asteroid.id;
		flaggedAsteroids.Add(ad.index, ad);
	}

	public ChunkData GetChunkData() {
		if (flaggedAsteroids.Count > 0) {
			UpdateAsteroidData ();

			ChunkData cd = new ChunkData();
			cd.asteroidData = new AsteroidData[flaggedAsteroids.Count];

			flaggedAsteroids.Values.CopyTo (cd.asteroidData, 0);
			cd.chunkx = chunkx;
			cd.chunky = chunky;
			return cd;
		}
		return null;
	}

	void UpdateAsteroidData() {
		foreach (KeyValuePair<int, Asteroid> p in asteroids) {
			if (p.Key < 0 && p.Value == null) {
				flaggedAsteroids.Remove (p.Key);
			} else if (p.Value == null) {
				flaggedAsteroids[p.Key].destroyed = true;
			} else if (p.Value.flagged) {
				AsteroidData ad = flaggedAsteroids[p.Key];
				ad.mineral = p.Value.mineral;
				ad.posx = p.Value.rigidbody2D.position.x;
				ad.posy = p.Value.rigidbody2D.position.y;
				ad.size = p.Value.sizeClass;
			}
		}
	}

	void SpawnAsteroidFromData(AsteroidData ad) {
		gen.transform.position = new Vector3(ad.posx,ad.posy,gen.transform.position.z);
		Asteroid clone = gen.GenerateAsteroid(ad.mineral, ad.size);
		clone.gen = gen;
		clone.chunk = this;
		clone.id = ad.index = asteroidIndex--;
		clone.flagged = true;

		asteroids.Add(clone.id, clone);
	}

	public void Load(AsteroidData[] ad) {
		flaggedAsteroids = new SortedList<int, AsteroidData>();
		foreach (AsteroidData a in ad) {
			flaggedAsteroids.Add (a.index, a);
		}
	}
}
