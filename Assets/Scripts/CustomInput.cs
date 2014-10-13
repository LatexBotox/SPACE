using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomInput : MonoBehaviour {
	KeyCode thrustKey = KeyCode.W;
	KeyCode dampenKey = KeyCode.LeftShift;
	KeyCode rotateLKey = KeyCode.A;
	KeyCode rotateRKey = KeyCode.D;
	KeyCode shootKey = KeyCode.Mouse0;
	KeyCode selectNextKey = KeyCode.UpArrow;
	KeyCode selectPrevKey = KeyCode.DownArrow;
	KeyCode enterKey = KeyCode.Return;
	KeyCode nextWeaponKey = KeyCode.E;
	KeyCode prevWeaponKey = KeyCode.Q;

	List<KeyListener> keyDownListeners;
	List<KeyListener> keyUpListeners;
	List<KeyListener> keyHeldListeners;

	public static CustomInput instance;

	void Start() {
		if(instance!=null) {
			Destroy (gameObject, 0f);
			return;
		}
		instance = this;
    DontDestroyOnLoad(gameObject);

		keyDownListeners = new List<KeyListener>();
		keyUpListeners	 = new List<KeyListener>();
		keyHeldListeners = new List<KeyListener>();
	}

	public KeyCode ThrustKey {
		get {
			return this.thrustKey;
		}
		set {
			thrustKey = value;
		}
	}

	public KeyCode DampenKey {
		get {
			return this.dampenKey;
		}
		set {
			dampenKey = value;
		}
	}

	public KeyCode RotateLKey {
		get {
			return this.rotateLKey;
		}
		set {
			rotateLKey = value;
		}
	}

	public KeyCode RotateRKey {
		get {
			return this.rotateRKey;
		}
		set {
			rotateRKey = value;
		}
	}

	public KeyCode ShootKey {
		get {
			return this.shootKey;
		}
		set {
			shootKey = value;
		}
	}

	public KeyCode SelectNextKey {
		get {
			return this.selectNextKey;
		}
		set {
			selectNextKey = value;
		}
	}

	public KeyCode SelectPrevKey {
		get {
			return this.selectPrevKey;
		}
		set {
			selectPrevKey = value;
		}
	}

	public KeyCode EnterKey {
		get {
			return this.enterKey;
		}
		set {
			enterKey = value;
		}
	}

	public KeyCode NextWeaponKey {
		get {
			return this.nextWeaponKey;
		}
		set {
			nextWeaponKey = value;
		}
	}

	public KeyCode PrevWeaponKey {
		get {
			return this.prevWeaponKey;
		}
		set {
			prevWeaponKey = value;
		}
	}


	
	public void RegDownListener(KeyListener kl) {
		keyDownListeners.Add(kl);
	}

	public void RegUpListener(KeyListener kl) {
		keyUpListeners.Add(kl);
	}

	public void RemDownListener(KeyListener kl) {
		keyDownListeners.Remove(kl);
	}

	public void RemUpListener(KeyListener kl) {
		keyUpListeners.Remove(kl);
	}

	
	public void RegHeldListener(KeyListener kl) {
		keyHeldListeners.Add(kl);
	}
	
	public void RemHeldListener(KeyListener kl) {
		keyHeldListeners.Remove(kl);
	}


	
	void Update() {
		foreach(KeyListener kl in keyDownListeners) {
			if(Input.GetKeyDown(kl.Key))
				kl.Callback();
		}

		foreach(KeyListener kl in keyUpListeners) {
			if(Input.GetKeyUp(kl.Key))
				kl.Callback();
		}

		foreach(KeyListener kl in keyHeldListeners) {
			if(Input.GetKey(kl.Key))
				kl.Callback();
		}
	}
}
