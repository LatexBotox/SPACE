﻿using UnityEngine;
using System.Collections;

public class MapController : MonoBehaviour {

  public static int mainSeed;
	public static MapController mc;
	public GraphGenerator generatorPrefab;
	public MapMothership mshipprefab;
	public LevelGenerator lvlgprefab;

  static int storedNodeIndex = -1;

	static GraphGenerator gg;
	GraphNode currentNode;
	MapMothership mship;
	int unlockedTier;

	void Start() {
		if (mc != null) {

			DestroyImmediate (gameObject);
			return;
		}
	
		mc = this;
		
		gg = Instantiate (generatorPrefab) as GraphGenerator;
		currentNode = gg.Generate ();


		if (storedNodeIndex != -1) {
			currentNode = gg.GetNode(storedNodeIndex);
		}

		currentNode.SetActive (true);
		Camera.main.transform.position = new Vector3 (currentNode.transform.position.x, currentNode.transform.position.y, -20);

		mship = Instantiate(mshipprefab, currentNode.transform.position, Quaternion.identity) as MapMothership;
		mship.SetState(MshipState.ORBIT, currentNode);
	}

	public void UnlockTier(int t) {
		unlockedTier = t;
	}

	public void NodeClicked(GraphNode gn) {
		if(!mship.IsOrbiting())
			return;

		if (gn.tier > unlockedTier) {
			return;
		}

		if (gn == currentNode)
			LoadWorld ();

		foreach(GraphNode g in gn.neighbours) {
			if(g == currentNode) {
				mship.SetState(MshipState.TRAVEL, gn);
				SetCurrentNode(gn);
			}
		}
	}

	public void SetCurrentNode(GraphNode gn) {
		if(currentNode != null)
			currentNode.SetActive(false);

		currentNode = gn;
		currentNode.SetActive(true);
	}

	public void LoadWorld() {

		currentNode.SetActive (false);
		storedNodeIndex = currentNode.index;
		LevelGenerator.tier = currentNode.tier;
		LevelGenerator.boss = currentNode.special;
		LevelGenerator.levelSeed = currentNode.seed;
		
		Application.LoadLevel("world");

	}

	/*void OnGUI() {
		if(mship.IsOrbiting()) {
			GUI.TextArea(new Rect(10, Screen.height - 90 , 400, 80), currentNode.GetNodeInfo());
			if(GUI.Button(new Rect(410, Screen.height - 40, 50, 20), "Enter")) {
                //ag = Instantiate(lvlgprefab) as LevelGenerator;
                //DontDestroyOnLoad(ag);
                //ag.tier = currentNode.tier;
                //ag.boss = currentNode.special;
                //ag.levelSeed = currentNode.seed;

                LevelGenerator.tier = currentNode.tier;
                LevelGenerator.boss = currentNode.special;
                LevelGenerator.levelSeed = currentNode.seed;


				Application.LoadLevel(2);
                //ag.enabled = true;
                //ShipBuilder.instance.Invoke("SpawnShip",0f);
			}
		}
	}*/
}
