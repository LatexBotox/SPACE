using UnityEngine;
using System.Collections;



public class GraphNode : MonoBehaviour {

	public ArrayList edges = new ArrayList();
	public ArrayList neighbours = new ArrayList();
	public int tier;
	public int seed;
	public int steps;
	public bool special;

	public void SetActive(bool a) {
		foreach(GraphEdge ge in edges) {
			ge.ShowColor(a);
		}
	}

	public string GetNodeInfo() {
		string s = "tier: " + tier + "\nseed: " + seed;
		return special ? s+"\nboss level!" : s;
	}

	void OnMouseDown() {
		MapController.mc.NodeClicked(this);
	}

	void Update() {
		if(special)
			Debug.DrawLine(transform.position, new Vector3(0,0,0));
	}
}