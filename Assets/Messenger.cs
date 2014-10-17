using UnityEngine;
using System.Collections;

public class Messenger : MonoBehaviour {


	public static Messenger instance;
	public GUIStyle style;

	ArrayList msgs;
	Message curMsg;

	Rect boxArea;
	Rect imgArea;
	Rect nameArea;
	Rect textArea;

	void Start() {
		instance = this;

		msgs = new ArrayList ();

		float boxWidth = 400;
		float boxHeight = 80;

		float imgWidth = 30;
		float imgHeight = 30;

		float nameWidth = 30;
		float nameHeight = 10;
		//
		boxArea = new Rect (Screen.width / 2 - boxWidth/2, Screen.height - 90, boxWidth, boxHeight);
		nameArea = new Rect(10, 5, nameWidth, nameHeight);
		imgArea = new Rect(10, 10 + nameHeight, imgWidth, imgHeight);
		textArea = new Rect (10 + imgWidth + 10, 10 + nameHeight, boxWidth - 10, boxHeight - 10);               
	}


	void Update() {
	
		if (curMsg != null) {
		
			if (Time.time > (curMsg.start_t + curMsg.duration)) {

				if (curMsg.callback != null) {
					curMsg.callback ();
					curMsg.voice.Stop();
					Destroy(curMsg.voice.gameObject, 0.1f);
				}

				curMsg = null;
			}

		} else {

			foreach (Message m in msgs) {
				if(Time.time > m.start_t) {
					m.start_t = Time.time;
					curMsg = m;
					curMsg.voice.Play();
					break;
				}
			}

			msgs.Remove(curMsg);

		}

	}

	void OnGUI() {

		if (curMsg != null) {
			GUI.BeginGroup(boxArea);
			GUI.Box(new Rect(0,0,boxArea.width, boxArea.height), "");
			GUI.DrawTexture(imgArea, curMsg.portrait);
			GUI.Label(nameArea, curMsg.speakerName);
			GUI.Label(textArea, curMsg.msg);
			GUI.EndGroup();
		}
	}


	public void AddMessage(Message msg) {
		msgs.Add (msg);
	}

	public void KillMessage(Message msg) {
		if (curMsg == msg) {
			curMsg.voice.Stop();
			Destroy(curMsg.voice.gameObject, 0.1f);
			curMsg = null;
		} else {
			msgs.Remove(msg);
		}
	}

}
