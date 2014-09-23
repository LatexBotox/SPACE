using UnityEngine;
using System.Collections;

public class MenuButton : MenuItem
{
	static int idcounter = 0;
	int myId;
	Rect a;
	Callback cb;
	string t;

	public int Id {
		get {
			return this.myId;
		}
		set {
			myId = value;
		}
	}
	public MenuButton(Rect area, string text, Callback callback) {
		a = area;
		cb = callback;
		t = text;
		myId = idcounter++;
	}

	public override void gUpdate() {
		if(!draw)
			return;

		if(GUI.Button(a, t)) {
			cb(myId);
		}
	}
}