using UnityEngine;
using System.Collections;



public class GraphNode : MonoBehaviour {
	public ArrayList edges = new ArrayList();
	public ArrayList neighbours = new ArrayList();

	public void SetActive(bool a) {
		foreach(GraphEdge ge in edges) {
			ge.SetActive(a);
		}
	}

	public string GetNodeInfo() {
		return "lol this be info";
	}

	void OnMouseDown() {
		MapController.mc.NodeClicked(this);
	}

	/*void OnMouseEnter() {
		foreach (GraphEdge ge in edges) {
			ge.SetActive(true);
		}
	}

	void OnMouseExit() {
		foreach (GraphEdge ge in edges) {
			ge.SetActive(false);
		}
	}*/

}
