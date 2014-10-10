using UnityEngine;
using System.Collections;



public class GraphNode : MonoBehaviour {

	public ArrayList edges = new ArrayList();
	public ArrayList neighbours = new ArrayList();
	public int tier;
	public int seed;
	public int steps;

	public void SetActive(bool a) {
		foreach(GraphEdge ge in edges) {
			ge.SetActive(a);
		}
	}

	public string GetNodeInfo() {
		return "tier: " + tier + "\nseed: " + seed;
	}

	void OnMouseDown() {
		MapController.mc.NodeClicked(this);
	}
}