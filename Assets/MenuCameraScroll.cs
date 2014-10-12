﻿using UnityEngine;
using System.Collections;

public class MenuCameraScroll : MonoBehaviour {
    public float scrollSpeed = 25;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = transform.position + new Vector3(0, 1, 0) * Time.fixedDeltaTime * scrollSpeed;
	}
}
