using UnityEngine;
using System.Collections;

public class MapController : MonoBehaviour {

	public static MapController mc;
	public GraphGenerator generatorPrefab;
	public MapMothership mshipprefab;

	GraphGenerator gg;
	GraphNode currentNode;
	MapMothership mship;

	void Start() {
		if( mc != null)
			return;

		mc = this;
		gg = Instantiate(generatorPrefab) as GraphGenerator;
		currentNode = gg.Generate ();

		mship = Instantiate(mshipprefab, currentNode.transform.position, Quaternion.identity) as MapMothership;
		mship.gn = currentNode;
	}

	public void NodeClicked(GraphNode gn) {
		if(!mship.IsOrbiting())
			return;

		foreach(GraphNode g in gn.neighbours) {
			if(g == currentNode) {
				mship.SetState(MshipState.TRAVEL);
				SetCurrentNode(gn);
			}
		}
	}

	public void SetCurrentNode(GraphNode gn) {
		currentNode.SetActive(false);
		currentNode = gn;
		currentNode.SetActive(true);
		mship.gn = gn;
	}

	void OnGUI() {
		if(mship.IsOrbiting()) {
			GUI.TextArea(new Rect(10, Screen.height - 90 , 400, 80), currentNode.GetNodeInfo());
			GUI.Button(new Rect(350, Screen.height - 40, 50, 20), "Enter"); 
		}
	}
}
