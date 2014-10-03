using UnityEngine;
using System.Collections;



public class GraphNode : MonoBehaviour {
	public ArrayList edges = new ArrayList();

	void OnMouseDown() {
		print ("lol clicked me");
	}

	void OnMouseEnter() {
		print ("amma gawd set me edges!");
		foreach (GraphEdge ge in edges) {
			ge.SetActive(true);
		}
	}

	void OnMouseExit() {
		print ("omgomgogm kill me edges");
		foreach (GraphEdge ge in edges) {
			ge.SetActive(false);
		}
	}

}
