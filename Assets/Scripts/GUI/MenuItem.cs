using UnityEngine;
using System.Collections;

public delegate void Callback(int id);

public abstract class MenuItem
{
	protected bool draw;

	public void SetDraw(bool d) {
		draw = d;
	}

	public abstract void gUpdate();

}

