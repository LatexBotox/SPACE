using UnityEngine;
using System.Collections;

public delegate void ColCallback(StateTrigger t);

public class TutorialMothership : MonoBehaviour
{
	public StateTrigger circleTrig;
	public StateTrigger boxTrig;

	void Start() {
		circleTrig.callback = TutorialController.instance.EnterCircle;
		boxTrig.callback = TutorialController.instance.EnterBox;

		SetBoxEnabled (false);
		SetCircleEnabled (true);
	}

	
	public void SetBoxEnabled(bool e) {
		boxTrig.SetEnabled (e);
	}

	public void SetCircleEnabled(bool e) {
		circleTrig.SetEnabled (e);
	}

	public void SetCircleCallback(ColCallback c) {
		circleTrig.callback = c;
	}

	
	public void SetBoxCallback(ColCallback c) {
		boxTrig.callback = c;
	}
}

