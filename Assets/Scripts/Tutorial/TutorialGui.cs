using UnityEngine;
using System.Collections;

public class TutorialGui : MonoBehaviour
{

	bool draw;
	string displayText;
	string speakerName;
	float end_t;
	TutorialState callbackState;

	void Start ()
	{
	}

	public void ShowText(string text, string speaker, float showtime, TutorialState state) {
		displayText = text;
		speakerName = speaker;
		end_t = Time.time + showtime;
		callbackState = state;
		draw = true;
		gameObject.SetActive(draw);
	}

	void Update() {
		if(!draw)
			gameObject.SetActive(draw);
	}


	void OnGUI() {
		if(Time.time > end_t) {
			callbackState.GuiCallback();
			draw = false;
		}

		GUI.TextArea(new Rect(10, Screen.height - 90 , 400, 80), speakerName + ":\n" + displayText);
	}

}

