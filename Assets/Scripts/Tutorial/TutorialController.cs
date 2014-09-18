using UnityEngine;
using System.Collections;

public class TutorialController : MonoBehaviour
{

	public ShipController p;
	public TutorialGui tGui;

	private Queue tStates;
	private TutorialState curState;

	public void HideMessage() {
		tGui.gameObject.SetActive(false);
	}

	public void DisplayMessage(string msg, string name, float time) {
		tGui.gameObject.SetActive(true);
		tGui.ShowText(msg, name, time, curState);
	}

	// Use this for initialization
	void Start ()
	{ 
		tStates = new Queue(3);
		curState = new Tstate1(p, this);
		tStates.Enqueue( new Tstate2(p, this));		
		curState.Start();
	}

	public void NextState() {
		curState = tStates.Dequeue() as TutorialState;
		curState.Start();
	}

	// Update is called once per frame
	void Update ()
	{
		curState.Update();
	}
}

