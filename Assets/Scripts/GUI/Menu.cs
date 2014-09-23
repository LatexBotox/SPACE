using UnityEngine;
using System.Collections;

public abstract class Menu
{
	public bool draw;
	protected Rect a;

	public virtual void SetDraw(bool d) {
		draw = d;
	}

	public abstract void gUpdate();

}

