using UnityEngine;
using System.Collections;

public class TutorialController : MonoBehaviour
{

	public GameObject mothership;

	public TutorialGui tGui;
	public TutorialState[] tStates;

	private TutorialState curState;
	private Queue qStates;

	public void SpawnMothership(Vector2 pos, Quaternion rot) {
		mothership.SetActive(true);
		mothership.transform.position = pos;
		mothership.transform.rotation = rot;
	}

	public void HideMessage() {
		tGui.HideText();
	}

	public void DisplayMessage(string msg, string name, float time) {
		tGui.ShowText(msg, name, time, curState);
	}

	public void TriggerEnter(StateTrigger t, Collider2D col)
	{
		curState.TriggerEnter(t, col);
	}

	void Start ()
	{ 
		mothership.SetActive(false);

		qStates = new Queue(tStates);
		NextState();
	}

	public void NextState() {
		curState = qStates.Dequeue() as TutorialState;
		curState.Run();
	}

	public void TutorialDone() {
		Application.LoadLevel(1);
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