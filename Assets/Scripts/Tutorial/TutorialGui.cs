using UnityEngine;
using System.Collections;

public class TutorialGui : MonoBehaviour
{

	public AudioSource a;

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
		a.volume = 1;
		a.Play();
		draw = true;
	}

	public void HideText() {
		draw = false;
	}


	void OnGUI() {

		if(!draw)
			return;

		if(Time.time > end_t) {
			draw = false;
			callbackState.GuiCallback();
		}

		GUI.TextArea(new Rect(10, Screen.height - 90 , 400, 80), speakerName + ":\n" + displayText);
	}
}

