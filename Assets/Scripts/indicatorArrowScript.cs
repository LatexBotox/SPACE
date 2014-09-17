using UnityEngine;
using System.Collections;

public class indicatorArrowScript : MonoBehaviour {

	public GameObject fromPos;
	public GameObject target;

	// Use this for initialization
	void Start () {
		SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
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
	void FixedUpdate () {
		if (!target || !fromPos) {
			DestroyImmediate(gameObject);
			return;
		}

		Vector2 relPos = fromPos.transform.position-target.transform.position;

		transform.rotation = Quaternion.AngleAxis (Mathf.Atan2(relPos.y, relPos.x)*Mathf.Rad2Deg,transform.forward);



		transform.localScale = new Vector3(3,Mathf.Lerp (6, 1, relPos.magnitude/300),1);

	}
}
