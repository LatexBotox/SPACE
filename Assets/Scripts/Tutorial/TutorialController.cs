using UnityEngine;
using System.Collections;

public class TutorialController : MonoBehaviour
{

	public static TutorialController instance;

	public TutorialGui tGui;
	public TutorialState[] tStates;

	private TutorialState curState;
	private Queue qStates;

	public void HideMessage() {
		tGui.HideText();
	}

	public void DisplayMessage(string msg, string name, float time) {
		tGui.ShowText(msg, name, time, curState);
	}

	public void EnterCircle(StateTrigger t) {
		curState.EnterCircle (t);
	}

	public void EnterBox(StateTrigger t) {
		curState.EnterBox (t);
	}

	void Start ()
	{ 
		instance = this;

    ShipBuilder.instance.SpawnShip();

		qStates = new Queue(tStates);
		NextState();
	}

	public void NextState() {
		curState = qStates.Dequeue() as TutorialState;
		curState.Run();
	}

	public void TutorialDone() {
		Application.LoadLevel("mapgentest");
	}

	void Update ()
	{
		if(curState.CheckExit()) {
			NextState();
			if(curState == null) {
				tGui.gameObject.SetActive(false);
				gameObject.SetActive(false);
				return;
			}
		}

		curState.sUpdate();
	}
}