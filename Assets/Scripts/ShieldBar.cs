using UnityEngine;
using System.Collections;

public class ShieldBar : MonoBehaviour {

	public int MaxWidth = 360;
	// Use this for initialization
	void Start () {
		Rect insets = guiTexture.pixelInset;
		insets.x = MaxWidth / -2;
		insets.width = MaxWidth;
		guiTexture.pixelInset = insets;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(PlayerShip.instance==null)
			return;

		Rect insets = guiTexture.pixelInset;

		insets.width = Mathf.CeilToInt ((PlayerShip.instance!=null?PlayerShip.instance.wings.Fraction ():0) * MaxWidth);

		guiTexture.pixelInset = insets;
	}
}
