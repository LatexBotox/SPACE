using UnityEngine;
using System.Collections;

public class TutorialController : MonoBehaviour
{

	public ShipController p;
	public TutorialGui tGui;

	private Queue tStates;
	private TutorialState curState;

	public void DisplayMessage(string msg, string name, float time) {
		tGui.ShowText(msg, name, time, curState);
	}

	// Use this for initialization
	void Start ()
	{ 
		print ("imma set my curstate");
		curState = new Tstate1(p, this);
		curState.Start();
	}

	public void NextState() {
		curState = tStates.Dequeue() as TutorialState;
	}

	// Update is called once per frame
	void Update ()
	{
		curState.Update();
	}
}

