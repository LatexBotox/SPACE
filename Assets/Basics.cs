using UnityEngine;
using System.Collections;

public class Basics : MonoBehaviour {
    static Basics instance;

	void Start () {
        if (instance != null)
        {
            Destroy(gameObject, 0f);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
	}
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.LoadLevel("mapgentest");
        }
    }
}
