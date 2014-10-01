using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

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

		Rect insets = guiTexture.pixelInset;
		insets.width = Mathf.CeilToInt ((PlayerShip.instance!=null?PlayerShip.instance.Fraction ():0) * MaxWidth);
		guiTexture.pixelInset = insets;
	}
}
