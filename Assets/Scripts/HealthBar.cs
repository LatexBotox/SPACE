using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public int MaxWidth = 360;
	public Destructables ship;

	// Use this for initialization
	void Start () {
		ship = GameObject.FindGameObjectWithTag("Player").GetComponent<Ship>();
		Rect insets = guiTexture.pixelInset;
		insets.x = MaxWidth / -2;
		insets.width = MaxWidth;
		guiTexture.pixelInset = insets;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Rect insets = guiTexture.pixelInset;
		insets.width = Mathf.CeilToInt ((!ship?0f:(float)ship.Fraction ()) * MaxWidth);
		guiTexture.pixelInset = insets;
	}
}
