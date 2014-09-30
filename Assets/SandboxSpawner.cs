using UnityEngine;
using System.Collections;

public class SandboxSpawner : MonoBehaviour {

	public Transform t;
	public EnemyShip[] enemies;

	int e_index;
	float spawnt = 0f;
	float spawndelay = 0.5f;
	KeyListener next, prev, spawn;


	void Start() {
		e_index = 0;
		next = new KeyListener(Stuff.input.SelectNextKey, this.NextEnemy, 0.2f);
		prev = new KeyListener(Stuff.input.SelectNextKey, this.PrevEnemy, 0.2f);
		spawn = new KeyListener(Stuff.input.EnterKey, this.SpawnEnemy, 0.2f);

		Stuff.input.RegDownListener(next);
		Stuff.input.RegDownListener(prev);
		Stuff.input.RegDownListener(spawn);
	}

	public void NextEnemy() {
			e_index = (++e_index)%enemies.Length;
	}

	public void PrevEnemy() {
		e_index = (--e_index)%enemies.Length;
	}

	public void SpawnEnemy() {
		Instantiate(enemies[e_index], t.position + t.up * 100, t.rotation);
	}
}
