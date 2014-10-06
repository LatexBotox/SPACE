using UnityEngine;
using System.Collections;
using System.Collections.Generic;


class NoSpaceException : System.Exception {}

public class GraphGenerator : MonoBehaviour {


	public GraphNode node;
	public GraphEdge edge;
	public Rect area;
	public float maxDist, minDist;
	public int seed;
	public int maxNrNodes;
	public float radius;


	Stack buildNodes;
	ArrayList allNodes;

	// Use this for initialization
	void Start () {

		buildNodes = new Stack ();
		allNodes = new ArrayList ();

		DFRandomWalk ();
	
	}
	
	void DFRandomWalk() {

		//First create the origin node at the position of the generator object
		Random.seed = seed;
		GraphNode origin = Instantiate (node, transform.position, Quaternion.identity) as GraphNode;
		allNodes.Add (origin);
		buildNodes.Push (origin);

		while (buildNodes.Count > 0 && allNodes.Count < maxNrNodes) {
			GraphNode currentNode = buildNodes.Peek () as GraphNode;
			Vector2 currentPos = currentNode.transform.position;

			//I dont like using a try catch block here since 
			//failing to find a next point is not exceptional
			//but for some reason i cant return null from FindNextPoint..
			try {
				//print ("attempting to create new node..");
				Vector2 nextPos = FindNextPoint (currentPos);
				GraphNode clone = Instantiate (node, nextPos, Quaternion.identity) as GraphNode;
				//print ("Successfully created node at: " + nextPos);

				BuildEdges (clone, FindNeighbours(nextPos));

				buildNodes.Push(clone);
				allNodes.Add(clone);
			} catch(NoSpaceException e) {
				buildNodes.Pop();
			}
		}

		print ("Finishied generating graph with " + allNodes.Count + " nodes");
	}

	void BuildEdges(GraphNode me, ArrayList others) {
		foreach (GraphNode gn in others) {
			GraphEdge ge = Instantiate(edge) as GraphEdge;
			ge.SetActive(false);
			ge.SetPositions(me.transform.position, gn.transform.position);
			me.edges.Add (ge);
			gn.edges.Add(ge);
		}
	}

	ArrayList FindNeighbours(Vector2 p) {
		ArrayList ret = new ArrayList ();
		SortedList candidates = new SortedList ();

		foreach (GraphNode gn in allNodes) {
			float distsq = (p - (Vector2)gn.transform.position).sqrMagnitude;
			if(distsq < maxDist*maxDist && gn.edges.Count < 4) {
				candidates.Add(distsq, gn);
			}
		}

		int count = 0;
		foreach (GraphNode gn in candidates.Values) {
			if(++count > 3)
				break;

			ret.Add(gn);
		}

		return ret;
	}

	Vector2 FindNextPoint(Vector2 p) {
		Vector2 dir = Random.insideUnitCircle.normalized;
		float mnitud = minDist + Random.value * (maxDist - minDist);

		for (int i=0; i<4; ++i) {
			Vector2 np = p+dir*mnitud;
			if(IsValid(np)) 
				return np;

			dir = new Vector2(dir.y, -dir.x);
		}

		throw new NoSpaceException ();
	}

	bool IsValid(Vector2 p) {

		/*if (!area.Contains (p)) {
			print ("Failed to create node at: " + p + "(out of bounds)");
			return false;

		}*/

		if ((p - (Vector2)transform.position).sqrMagnitude > radius * radius) {
			return false;
		}


		foreach (GraphNode gn in allNodes) {
			float dist = (p - (Vector2)gn.transform.position).sqrMagnitude;
			if(dist < minDist*minDist) {
				return false;
			}
		}

		return true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
