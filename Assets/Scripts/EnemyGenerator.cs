using UnityEngine;
using System.Collections;

public class EnemyGenerator : MonoBehaviour {

	public EnemyShip[] enemies;
	public float[] enemiesChance;

	public float spawnChance = 0.01f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public EnemyShip GenerateRandomEnemy(int x, int y, int seed) {
		Random.seed = (int)Time.realtimeSinceStartup + seed;
		float totalChance = 0;

		foreach (float f in enemiesChance)
			totalChance+=f;

		float ran = Random.Range (0f,totalChance*spawnChance);
		float accu = 0;

		EnemyShip clone = null;
		transform.position = new Vector2(x, y);

		for (int i = 0;i<enemies.Length;i++) {
			accu+=enemiesChance[i];
			if(accu>ran) {
				clone = Instantiate(enemies[i],transform.position,enemies[i].transform.rotation) as EnemyShip;
				clone.DespawnIn(15);
				break;
			}
		}



		return clone;
	}
}
