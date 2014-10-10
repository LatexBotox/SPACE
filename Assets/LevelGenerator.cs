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
	EnemyGenerator enemyGen;

	public Chunk chunk;
	Chunk[,] initializedChunks;
	public int levelSize = 256;
	int chunkSize = 128;
	public int levelSeed;

	// Use this for initialization
	void Start () {
		asteroidGen = gameObject.GetComponentInChildren<AsteroidGenerator>();
		enemyGen = gameObject.GetComponentInChildren<EnemyGenerator>();
		//enabled = player = GameObject.FindGameObjectWithTag("Player");



		float t = 0 - Time.realtimeSinceStartup;

		asteroidGen.levelSeed = levelSeed;

		initializedChunks = new Chunk[levelSize, levelSize];
		Load ();
		t += Time.realtimeSinceStartup;

		//print ("Loaded Chunks in: " + t);
	}

	void FixedUpdate () {


		if(!PlayerShip.instance)
			return;

		player = PlayerShip.instance.gameObject;
		
		for (int y = Mathf.FloorToInt(player.transform.position.y/chunkSize)-6;y <= Mathf.FloorToInt(player.transform.position.y/chunkSize)+6;y++) {
			for (int x = Mathf.FloorToInt(player.transform.position.x/chunkSize)-6;x <= Mathf.FloorToInt(player.transform.position.x/chunkSize)+6;x++) {
				int _x = Mathf.Clamp (x+levelSize/2,0, levelSize-1);
				int _y = Mathf.Clamp (y+levelSize/2,0, levelSize-1);

				if (!initializedChunks[_y,_x]) {
					initializedChunks[_y,_x] = Instantiate (chunk, new Vector2(x*chunkSize, y*chunkSize),chunk.transform.rotation) as Chunk;
					initializedChunks[_y,_x].transform.parent = transform;
				}
			}
		}


		foreach(Collider2D c in Physics2D.OverlapCircleAll(player.transform.position, chunkSize*4, 1<<15)) {
			chunk = c.GetComponent<Chunk>();
			if (chunk) {
				chunk.Generate();
			}
		}

		foreach(Collider2D c in Physics2D.OverlapCircleAll (player.transform.position, chunkSize*4, 1<<9)) {
			EnemyShip e = c.GetComponent <EnemyShip>();
			if(e!=null)
				e.DespawnIn(15);
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
				if (c!= null) {
					Chunk loadedChunk = initializedChunks[c.chunky+levelSize/2, c.chunkx+levelSize/2] = 
						Instantiate (chunk, new Vector2(c.chunkx*chunkSize, c.chunky*chunkSize), chunk.transform.rotation) as Chunk;

					loadedChunk.Load (c.asteroidData);
				}
			}
		}
	}

}
