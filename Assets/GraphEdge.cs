using UnityEngine;
using System.Collections;

public class GraphEdge : MonoBehaviour {
	public Color activeC, inactiveC;
	public bool active;

	public void SetActive(bool a) {
		LineRenderer rend = gameObject.GetComponent<LineRenderer> ();

		print ("herp diderp set me");

		if (a) {
			rend.SetColors (activeC, activeC);
		}	else  {
			rend.SetColors (inactiveC, inactiveC);
		}
	}

	public void SetPositions(Vector2 pos1, Vector2 pos2) {

		LineRenderer rend = gameObject.GetComponent<LineRenderer> ();
		rend.SetPosition (0, pos1);
		rend.SetPosition (1, pos2);
	}
}
