using UnityEngine;
using System.Collections;

public class CameraHandler : MonoBehaviour
{
	public Camera mainCam;
	public Camera hangarCam;
	public GUIHangar hGui;

	void Start() {
		hGui.gameObject.SetActive(false);
		mainCam.gameObject.SetActive(true);
		hangarCam.gameObject.SetActive(false);
	}

	public void ActivateMainCam() {
		hGui.gameObject.SetActive(false);
		mainCam.gameObject.SetActive(true);
		hangarCam.gameObject.SetActive(false);
	}

	public void ActivateHangarCam() {
		hGui.gameObject.SetActive(true);
		mainCam.gameObject.SetActive(false);
		hangarCam.gameObject.SetActive(true);
	}
}

