using UnityEngine;
using System.Collections;

public class IndicatorArrow : MonoBehaviour {

	public GameObject fromPos;
	public GameObject target;
	SpriteRenderer spriteRenderer;
	public float maxRange = 200;
	// Use this for initialization
	void Start () {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		if (target.layer == LayerMask.NameToLayer ("Ally")) {
			spriteRenderer.color = Color.green;
		} else if (target.layer == LayerMask.NameToLayer ("Enemy")) {
			spriteRenderer.color = Color.red;
		} else {
			spriteRenderer.color = Color.yellow;
		}

		if (!target) {
			print ("OFF");
			enabled = false;
		}
	}

	void SetTarget(GameObject t) {
		target = t;
		enabled = true;
	}

	// Update is called once per frame
	public void rotate() {
		Vector2 relPos = fromPos.transform.position-target.transform.position;
		
		transform.rotation = Quaternion.AngleAxis (Mathf.Atan2(relPos.y, relPos.x)*Mathf.Rad2Deg,transform.forward);

		transform.localScale = new Vector3(3,Mathf.Lerp (6, 0.2f, relPos.magnitude/maxRange),1);
	}

	void FixedUpdate () {
		if (!target || !fromPos) {
			DestroyImmediate(gameObject);
			return;
		}

		rotate ();

		Vector2 relPos = fromPos.transform.position-target.transform.position;
		spriteRenderer.color = new Color(spriteRenderer.color.r,
		                                 spriteRenderer.color.g,
		                                 spriteRenderer.color.b,
		                                 Mathf.Lerp (0, 1, (relPos.magnitude-25) / 28));
	}
}
