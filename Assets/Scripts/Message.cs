using UnityEngine;
using System.Collections;

public delegate void MsgCallback();

public class Message
{
	AudioSource _voice;
	string _msg;
	string _speakerName;
	Texture _portrait;

	float _start_t;
	float _duration;

	MsgCallback _callback;


	public MsgCallback callback {
		get {
			return _callback;
		}
	}

	public float duration {
		get {
			return _duration;
		}
	}

	public Texture portrait {
		get {
			return _portrait;
		}
	}

	public string speakerName {
		get {
			return _speakerName;
		}
	}

	public float start_t {
		get {
			return _start_t;
		}
		set {
			_start_t = value;
		}
	}

	public AudioSource voice {
		get {
			return _voice;
		}
	}

	public string msg {
		get {
			return _msg;
		}
	}

	public Message (string msg, string speakerName, float delay=0f, float duration=6f, Texture portrait=null, AudioSource voice=null, MsgCallback callback=null)
	{
		this._voice = voice;
		this._msg = msg;
		this._speakerName = speakerName;
		this._portrait = portrait;
		this._start_t = Time.time + delay;
		this._duration = duration;
		this._callback = callback;

		if (_voice == null) {
			GameObject gv = Resources.Load ("DefaultVoice") as GameObject;
			GameObject clone = GameObject.Instantiate(gv) as GameObject;
			_voice = clone.GetComponent<AudioSource>();
		}

		if (_portrait == null) {
			_portrait = Resources.Load("DefaultPortrait") as Texture;
		}
	}
}