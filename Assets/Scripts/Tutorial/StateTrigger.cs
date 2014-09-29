using UnityEngine;
using System.Collections;

public class StateTrigger : MonoBehaviour
{

	public TutorialController tc;

	void OnTriggerEnter2D(Collider2D col) {
		if(col.tag != "Player")
			return;
		
		tc.TriggerEnter(this, col);
	}
}

