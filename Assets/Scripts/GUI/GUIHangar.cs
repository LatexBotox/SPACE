using UnityEngine;
using System.Collections;

public class GUIHangar : MonoBehaviour
{
	public MenuFactory mf;
	Menu m;

	void Start() {
		m = mf.CreateMenu();
		m.SetDraw(true);
	}

	void OnGUI () {
		m.gUpdate();
	}
}
