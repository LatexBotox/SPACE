using UnityEngine;
using System.Collections;

public abstract class TutorialState : MonoBehaviour
{
	public TutorialController tControl;
	protected bool exit = false;

	public virtual void EnterCircle(StateTrigger t) {}
	public virtual void EnterBox(StateTrigger t) {}
	public virtual void sUpdate() {}
	public virtual bool CheckExit() { return exit; }
	public virtual void GuiCallback() {} 
	public abstract void Run();
}

