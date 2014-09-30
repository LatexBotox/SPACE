using UnityEngine;
using System.Collections;

public class ShieldImpact : MonoBehaviour {
	SpriteRenderer sprite;

	void Start() {
		print ("IMPACT");
		sprite = GetComponent<SpriteRenderer>();
		Destroy(gameObject, 2f);
	}

	void Update () {
		Color c = sprite.color;
		c -= new Color(0,0,0,Time.deltaTime*1f);
		sprite.color = c;
	}
}
