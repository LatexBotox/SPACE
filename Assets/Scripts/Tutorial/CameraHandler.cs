using UnityEngine;
using System.Collections;

public class CameraHandler : MonoBehaviour
{
	public Camera mainCam;
	public Camera hangarCam;

	void Start() {
		mainCam.gameObject.SetActive(true);
		hangarCam.gameObject.SetActive(false);
	}

	public void ActivateMainCam() {
		mainCam.gameObject.SetActive(true);
		hangarCam.gameObject.SetActive(false);
	}

	public void ActivateHangarCam() {
		mainCam.gameObject.SetActive(true);
		hangarCam.gameObject.SetActive(false);
	}
}

