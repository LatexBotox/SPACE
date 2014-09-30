using UnityEngine;
using System.Collections;

public delegate void KeyCallback();

public class KeyListener
{
	KeyCode key;
	float delay;
	float tactivated = 0;
	KeyCallback kc;

	public KeyCode Key {
		get {
			return this.key;
		}
	}

/*	public KeyListener(KeyCode _key, KeyCallback _kc) {
		key = _key;
		delay = 0.0f;
		kc = _kc;
	}
*/
	public KeyListener(KeyCode _key, KeyCallback _kc, float _delay = 0.0f) {
		key = _key;
		delay = _delay;
		kc = _kc;
	}

	public void Callback() {
		if(Time.time - tactivated > delay) {
			kc();
			tactivated = Time.time;
		}
	}
}

