using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {

	GameObject player;
	AsteroidGenerator asteroidGen;

	public Chunk chunk;
	bool[,] initializedChunks;
	public int levelSize = 256;
	int chunkSize = 128;

	// Use this for initialization
	void Start () {
		asteroidGen = gameObject.GetComponentInChildren<AsteroidGenerator>();
		enabled = player = GameObject.FindGameObjectWithTag("Player");



		float t = 0 - Time.realtimeSinceStartup;

		initializedChunks = new bool[levelSize, levelSize];

//		GameObject clone;
//
//		for(float y = -levelSize/2;y < levelSize/2;y++) {
//			for(float x = -levelSize/2;x < levelSize/2;x++) {
//				Instantiate (chunk, new Vector2(y*chunkSize, x*chunkSize),chunk.transform.rotation);
//			}
//		}
		t += Time.realtimeSinceStartup;

		print ("Generated Empty Chunks in: " + t);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(!player)
			return;

		for (int y = Mathf.FloorToInt(player.transform.position.y/chunkSize)-6;y <= Mathf.FloorToInt(player.transform.position.y/chunkSize)+6;y++) {
			for (int x = Mathf.FloorToInt(player.transform.position.x/chunkSize)-6;x <= Mathf.FloorToInt(player.transform.position.x/chunkSize)+6;x++) {
				if (!initializedChunks[y+levelSize/2,x+levelSize/2]) {
					Instantiate (chunk, new Vector2(x*chunkSize, y*chunkSize),chunk.transform.rotation);
					initializedChunks[y+levelSize/2,x+levelSize/2] = true;
				}
			}
		}


		foreach(Collider2D c in Physics2D.OverlapCircleAll(player.transform.position, chunkSize*4, 1<<15)) {
			chunk = c.GetComponent<Chunk>();
			if (chunk)
				chunk.Generate();
		}
	}
}
