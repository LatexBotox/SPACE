using UnityEngine;
using System.Collections;



public class GraphNode : MonoBehaviour {
	public ArrayList edges = new ArrayList();

	void OnMouseDown() {
	}

	void OnMouseEnter() {
		foreach (GraphEdge ge in edges) {
			ge.SetActive(true);
		}
	}

	void OnMouseExit() {
		foreach (GraphEdge ge in edges) {
			ge.SetActive(false);
		}
	}

}
