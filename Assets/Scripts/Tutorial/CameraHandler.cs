using UnityEngine;
using System.Collections;

public class CameraHandler : MonoBehaviour
{
	public PlayerShip pl;
	public Camera mainCam;
	public Camera hangarCam;
	public GUIHangar hGui;

	void Start() {
		hGui.gameObject.SetActive(false);
		mainCam.gameObject.SetActive(true);
		hangarCam.gameObject.SetActive(false);
	}

	public void ActivateMainCam() {
		PlayerShip.instance.gameObject.SetActive(true);
		hGui.gameObject.SetActive(false);
		mainCam.gameObject.SetActive(true);
		hangarCam.gameObject.SetActive(false);
	}

	public void ActivateHangarCam() {
		PlayerShip.instance.gameObject.SetActive(false);
		hGui.gameObject.SetActive(true);
		mainCam.gameObject.SetActive(false);
		hangarCam.gameObject.SetActive(true);
	}
}

