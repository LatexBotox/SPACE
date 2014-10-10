using UnityEngine;
using System.Collections;

public class GraphEdge : MonoBehaviour {
	public Color activeC, inactiveC;
	public bool active;

	public void SetActive(bool a) {
		if (a) {
			renderer.material.SetColor ("_Color", activeC);
		}	else  {
			renderer.material.SetColor ("_Color", inactiveC);
		}
	}

	public void SetPositions(Vector2 pos1, Vector2 pos2) {

		LineRenderer rend = gameObject.GetComponent<LineRenderer> ();
		rend.SetPosition (0, pos1);
		rend.SetPosition (1, pos2);
	}
}
