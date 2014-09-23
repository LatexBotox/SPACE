using UnityEngine;
using System.Collections;

public class ParticleSuicide : MonoBehaviour {
	void Start () {
		Destroy(gameObject, particleSystem.duration);
	}
}
