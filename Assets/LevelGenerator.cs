using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;

[Serializable]
public class Level {
	public ChunkData[] chunks; 
}


public class LevelGenerator : MonoBehaviour {

	GameObject player;
	AsteroidGenerator asteroidGen;

	public Chunk chunk;
	Chunk[,] initializedChunks;
	public int levelSize = 256;
	int chunkSize = 128;
	public int levelSeed;

	// Use this for initialization
	void Start () {
		asteroidGen = gameObject.GetComponentInChildren<AsteroidGenerator>();
		enabled = player = GameObject.FindGameObjectWithTag("Player");



		float t = 0 - Time.realtimeSinceStartup;

		asteroidGen.levelSeed = levelSeed;

		initializedChunks = new Chunk[levelSize, levelSize];
		Load ();

//		GameObject clone;
//
//		for(float y = -levelSize/2;y < levelSize/2;y++) {
//			for(float x = -levelSize/2;x < levelSize/2;x++) {
//				Instantiate (chunk, new Vector2(y*chunkSize, x*chunkSize),chunk.transform.rotation);
//			}
//		}
		t += Time.realtimeSinceStartup;

		print ("Loaded Chunks in: " + t);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(!player)
			return;

		for (int y = Mathf.FloorToInt(player.transform.position.y/chunkSize)-6;y <= Mathf.FloorToInt(player.transform.position.y/chunkSize)+6;y++) {
			for (int x = Mathf.FloorToInt(player.transform.position.x/chunkSize)-6;x <= Mathf.FloorToInt(player.transform.position.x/chunkSize)+6;x++) {
				if (!initializedChunks[y+levelSize/2,x+levelSize/2]) {
					initializedChunks[y+levelSize/2,x+levelSize/2] = Instantiate (chunk, new Vector2(x*chunkSize, y*chunkSize),chunk.transform.rotation) as Chunk;
				}
			}
		}


		foreach(Collider2D c in Physics2D.OverlapCircleAll(player.transform.position, chunkSize*4, 1<<15)) {
			chunk = c.GetComponent<Chunk>();
			if (chunk)
				chunk.Generate();
		}
	}

	void Update () {
		if (Input.GetKeyUp(KeyCode.Escape)) {
			Save ();
			Application.Quit ();
		}
	}

	void Save() {
		print ("Saving...");
		List<ChunkData> chunkData = new List<ChunkData>();

		foreach (Chunk chunk in initializedChunks) {
			if (chunk!= null) {
				ChunkData cd = chunk.GetChunkData ();
				if (cd != null) 
					print ("Saved chunk at pos " + cd.chunkx + ", " + cd.chunky);
					chunkData.Add(cd);
			}
		}

		Level level = new Level();
		level.chunks = chunkData.ToArray ();

		BinaryFormatter bf = new BinaryFormatter();

		System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/levels/");

		FileStream fs = File.Create (Application.persistentDataPath + "/levels/" + asteroidGen.levelSeed + ".lvl");

		bf.Serialize (fs, level);
		fs.Close ();
	}

	void Load() {
		if (File.Exists (Application.persistentDataPath + "/levels/" + asteroidGen.levelSeed + ".lvl")) {
			print ("Loading...");
			BinaryFormatter bf = new BinaryFormatter();
			FileStream fs = File.Open (Application.persistentDataPath + "/levels/" + asteroidGen.levelSeed + ".lvl", FileMode.Open);
			Level level = (Level)bf.Deserialize (fs);
			fs.Close();

			foreach (ChunkData c in level.chunks) {
				if(c!=null) {
					print ("Loaded chunk at pos " + c.chunkx + ", " + c.chunky);

					initializedChunks[c.chunky+levelSize/2, c.chunkx+levelSize/2] = Instantiate (chunk, new Vector2(c.chunkx*chunkSize, c.chunky*chunkSize), chunk.transform.rotation) as Chunk;
					initializedChunks[c.chunky+levelSize/2, c.chunkx+levelSize/2].flaggedAsteroids = c.asteroidData.OfType<AsteroidData>().ToList();
					
				}
			}
		}
	}
}
