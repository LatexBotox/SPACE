using UnityEngine;
using System.Collections;

public class StateTrigger : MonoBehaviour
{

	void OnTriggerEnter2D(Collider2D col) {
		if(col.tag != "Player")
			return;

		print("omg trigger");
		GameObject g = GameObject.FindWithTag("tutCont");
		TutorialController tc = g.GetComponent<TutorialController>();
		tc.TriggerEnter(this, col);
	}
}

