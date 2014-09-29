using UnityEngine;
using System.Collections;

public class ShieldBar : MonoBehaviour {

	public int MaxWidth = 360;
	public Ship ship;
	Wings wings;
	// Use this for initialization
	void Start () {
		Rect insets = guiTexture.pixelInset;
		insets.x = MaxWidth / -2;
		insets.width = MaxWidth;
		guiTexture.pixelInset = insets;

		wings = ship.GetComponentInChildren<Wings>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Rect insets = guiTexture.pixelInset;
		insets.width = Mathf.CeilToInt ((!wings?0f:wings.Fraction ()) * MaxWidth);
		guiTexture.pixelInset = insets;
	}
}
