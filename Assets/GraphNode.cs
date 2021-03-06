﻿using UnityEngine;
using System.Collections;



public class GraphNode : MonoBehaviour {

	public ArrayList edges = new ArrayList();
	public ArrayList neighbours = new ArrayList();
	public int tier;
	public int seed;
	public int steps;
	public bool special;
	public int index;

	bool drawInfo;
	Vector3 oldScale;

	public void SetActive(bool a) {
		foreach(GraphEdge ge in edges) {
			ge.ShowColor(a);
		}
	}

	public string GetNodeInfo() {
		string s = "tier: " + tier;
		return special ? s+"\nboss!" : s;
	}

	void OnMouseEnter() {
		drawInfo = true;

		oldScale = transform.localScale;
		Vector3 newScale = new Vector3 (oldScale.x * 2f, oldScale.y * 2f, oldScale.z);
		transform.localScale = newScale;
	}

	void OnMouseExit() {
		drawInfo = false;
		transform.localScale = oldScale;
	}

	void OnMouseDown() {
		MapController.mc.NodeClicked(this);
	}

	void Update() {
		if(special)
			Debug.DrawLine(transform.position, new Vector3(0,0,0));
	}

	void OnGUI() {
		if (!drawInfo)
			return;
	
		float w = 60f;
		float h = 70f;
		Rect posr = new Rect (Event.current.mousePosition.x - w, Event.current.mousePosition.y - h, w, h);
		GUI.Box (posr, GetNodeInfo());
	
	}
}