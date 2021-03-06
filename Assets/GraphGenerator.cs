﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;



class NoSpaceException : System.Exception {}


class NodeComp : IComparer<Collider2D> {
	Vector2 point;

	public NodeComp(Vector2 p) {
		point = p;
	}

	public int Compare(Collider2D c1, Collider2D c2) {
		if (((Vector2)c1.transform.position - point).sqrMagnitude <
			((Vector2)c2.transform.position - point).sqrMagnitude)
			return -1;
		else if (((Vector2)c2.transform.position - point).sqrMagnitude <
			((Vector2)c1.transform.position - point).sqrMagnitude)
			return 1;
		else
			return 0;
	}
}


public class GraphGenerator : MonoBehaviour {

	public GraphNode node;
	public GraphEdge edge;
	public float maxDist, minDist;
	public int seed;
	public int maxNrNodes;
	public float radius;
	public int nrtiers;
	public int maxsteps;

	Stack buildNodes = new Stack();
	ArrayList allNodes = new ArrayList();
	ArrayList[] tierList;

	public GraphNode Generate() {
		nrtiers = 4;
		tierList = new ArrayList[nrtiers];
		for(int i=0; i<nrtiers; ++i)
			tierList[i] = new ArrayList();

		return DFRandomWalk ();
	}

	public GraphNode GetNode(int index) {
		return allNodes [index] as GraphNode;
	}
	
	GraphNode DFRandomWalk() {

		//First create the origin node at the position of the generator object
		int index = 0;
		Random.seed = seed;
		GraphNode origin = Instantiate (node, transform.position, Quaternion.identity) as GraphNode;
		origin.index = index;
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

				ArrayList nbrs = FindNeighbours(nextPos);
				BuildEdges (clone, nbrs);

				foreach(GraphNode gn in nbrs) {
					gn.neighbours.Add(clone);
				}

				clone.index = ++index;
				clone.neighbours.AddRange(nbrs);
				buildNodes.Push(clone);
				allNodes.Add(clone);
			} catch(NoSpaceException e) {
				buildNodes.Pop();
			}
		}

		CalcTiers(origin);
		PickSpecials();

		print ("Finishied generating graph with " + allNodes.Count + " nodes");
		return origin;
	}

	void PickSpecials() {
		foreach(ArrayList l in tierList) {
			int randindex = Random.Range(0, l.Count);
			GraphNode gn = l.ToArray()[randindex] as GraphNode;
			gn.special = true;
		}
	}

	void CalcTiers(GraphNode origin) {
		Queue frontier = new Queue();
		int maxStep = 0; 

		ArrayList visited = new ArrayList();
		 
		origin.steps = 0;
		frontier.Enqueue(origin);

		while(frontier.Count > 0) {

			GraphNode gn = (GraphNode)frontier.Dequeue();
			visited.Add(gn);

			foreach(GraphNode g in gn.neighbours) {
				if(!visited.Contains(g)) {
					maxStep = gn.steps+1>maxStep ? gn.steps+1 : maxStep;
					g.steps = maxStep+1;
					frontier.Enqueue(g);
				}
			}
		}
	
		int range =  maxStep / nrtiers;
		foreach(GraphNode gn in visited) {
			for(int i=0; i<nrtiers; i++) {
				if(i*range <= gn.steps && gn.steps <= (i+1)*range) {
					gn.tier = i;
					gn.seed = Random.Range(0, int.MaxValue);
					tierList[i].Add(gn);
					break;
				}
			}
		}
	}

	void BuildEdges(GraphNode me, ArrayList others) {
		foreach (GraphNode gn in others) {
			GraphEdge ge = Instantiate(edge) as GraphEdge;
			ge.ShowColor(false);
			ge.SetPositions(me.transform.position, gn.transform.position);
			me.edges.Add (ge);
			gn.edges.Add(ge);
		}
	}


	ArrayList FindNeighbours(Vector2 p) {
		ArrayList ret = new ArrayList ();
		List<Collider2D> results = new List<Collider2D>();

		
		results.AddRange(Physics2D.OverlapCircleAll(p, maxDist, 1 << 16)); 
		results.Sort( new NodeComp(p) );

		/*foreach (GraphNode gn in allNodes) {
			float distsq = (p - (Vector2)gn.transform.position).sqrMagnitude;
			if(distsq < maxDist*maxDist && gn.edges.Count < 4) {
				candidates.Add(distsq, gn);
			}
		}*/

		int count = 0;
		foreach (Collider2D gn in results) {
			if(++count > 3)
				break;

			ret.Add(gn.GetComponent<GraphNode>());
		}


		//print ("nr neighbours: " + ret.Count);
		return ret;
	}

	Vector2 FindNextPoint(Vector2 p) {
		Vector2 dir = Random.insideUnitCircle.normalized;
		float mnitud = minDist + Random.value * (maxDist - minDist) + node.transform.localScale.x;

		for (int i=0; i<4; ++i) {
			Vector2 np = p+dir*mnitud;
			if(IsValid(np)) 
				return np;
			//
			dir = new Vector2(dir.y, -dir.x);
		}
		
		throw new NoSpaceException ();
	}

	bool IsValid(Vector2 p) {
	
		if ((p - (Vector2)transform.position).sqrMagnitude > radius * radius) {
			return false;
		}

	
		Collider2D col = Physics2D.OverlapCircle (p, minDist, 1 << 16);
		if (col != null) {
			//print ("Failed to create node at: " + p + "(too close to others) " + col.name);
			return false;
		}

		return true;
	}
}
