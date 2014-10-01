using UnityEngine;
using System.Collections;

public class SandboxSpawner : MonoBehaviour {

	public EnemyShip[] enemies;

	int e_index;
	float spawnt = 0f;
	float spawndelay = 0.5f;
	KeyListener next, prev, spawn;


	void Start() {
		e_index = 0;
	}

	void FixedUpdate() {
		try {
			next = new KeyListener(CustomInput.instance.SelectNextKey, this.NextEnemy, 0.2f);
			prev = new KeyListener(CustomInput.instance.SelectNextKey, this.PrevEnemy, 0.2f);
			spawn = new KeyListener(CustomInput.instance.EnterKey, this.SpawnEnemy, 0.2f);
			
			CustomInput.instance.RegDownListener(next);
			CustomInput.instance.RegDownListener(prev);
			CustomInput.instance.RegDownListener(spawn);
			enabled = false;
		} catch (UnityException e) {
			
		} 
	}

	public void NextEnemy() {
			e_index = (++e_index)%enemies.Length;
	}

	public void PrevEnemy() {
		e_index = (--e_index)%enemies.Length;
	}

	public void SpawnEnemy() {
		Instantiate(enemies[e_index], PlayerShip.instance.transform.position + PlayerShip.instance.transform.up * 100, PlayerShip.instance.transform.rotation);
	}
}
