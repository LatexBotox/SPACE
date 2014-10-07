using UnityEngine;
using System.Collections;

public enum MshipState {
	ORBIT, TRAVEL
};

public class MapMothership : MonoBehaviour {
	public GraphNode gn;
	float orbitDist = 10.0f;
	float orbitTargetRotateSpeed = 0.2f;
	float orbitRotateSpeed = 100.0f;
	float orbitMoveSpeed = 500.0f;
	float travelRotateSpeed = 100.0f;
	float travelMoveSpeed = 1000.0f;
	
	MshipState state;
	Vector2 orbitVec;	
	//Vector2 targetPos;

	void Start() {
		orbitVec = new Vector2(1.0f, 0.0f);
		state = MshipState.ORBIT;
	}

	public void SetState(MshipState s) {
		print ("set state " + s);
		state = s;
	}

	public bool IsOrbiting() {
		return state == MshipState.ORBIT;
	}

	void MoveToPos(Vector2 targetPos, float mspeed, float rspeed) {
	

		
//		Debug.DrawLine(transform.position, (Vector2)transform.position + moveDir.normalized*15.0f);
		//Debug.DrawLine(gn.transform.position, targetPos);
	}

	void Orbit() {

		//moving
	

		float rotAngle = Mathf.PI * orbitTargetRotateSpeed * Time.deltaTime;

		float sin = Mathf.Sin(rotAngle);
		float cos = Mathf.Cos(rotAngle);

		float nx = orbitVec.x * cos - orbitVec.y * sin;
		float ny = orbitVec.x * sin + orbitVec.y * cos;

		orbitVec = new Vector2(nx, ny);
		Vector2 targetPos = (Vector2)gn.transform.position + orbitVec*orbitDist;

		//MoveToPos(targetPos, orbitMoveSpeed, orbitRotateSpeed);
		Vector2 forward = -transform.up.normalized;
		Vector2 left = transform.right.normalized;
		
		Vector2 moveDir = (targetPos - (Vector2)transform.position); //.normalized;
		Vector2 f = moveDir * orbitMoveSpeed * Time.deltaTime;
		
		//rigidbody2D.AddForce(f);
		rigidbody2D.velocity = f;
		//rigidbody2D.position = targetPos;
		//rotation
		
		float angle = Vector2.Angle(forward, moveDir.normalized);
		
		if(angle > 0.0f) {
			float dot = Vector2.Dot (moveDir.normalized, left);
			if(dot > 0.0f) {
				rigidbody2D.angularVelocity = orbitRotateSpeed*angle * Time.deltaTime;
				//rigidbody2D.AddTorque(-orbitRotateSpeed*angle * Time.deltaTime);
			}	else {
				//rigidbody2D.AddTorque(orbitRotateSpeed*angle * Time.deltaTime);
				rigidbody2D.angularVelocity = -orbitRotateSpeed*angle * Time.deltaTime;
			}
		}




	}

	void Travel() {
		Vector2 targetPos = gn.transform.position;

		Vector2 forward = -transform.up.normalized;
		Vector2 left = transform.right.normalized;
		
		Vector2 moveDir = (targetPos - (Vector2)transform.position); //.normalized;
		float magnitude = moveDir.magnitude;
		moveDir = moveDir.normalized;

		if(magnitude < orbitDist) {
			SetState(MshipState.ORBIT);
			orbitVec = -moveDir;
			return;
		}

		Vector2 f = moveDir * travelMoveSpeed * Time.deltaTime;
		
		//rigidbody2D.AddForce(f);
		rigidbody2D.velocity = f;
		//rigidbody2D.position += f;
		//rotation
		
		float angle = Vector2.Angle(forward, moveDir.normalized);
		
		if(angle > 0.0f) {
			float dot = Vector2.Dot (moveDir.normalized, left);
			if(dot > 0.0f) {
				rigidbody2D.angularVelocity = travelRotateSpeed*angle * Time.deltaTime;
				//rigidbody2D.AddTorque(-orbitRotateSpeed*angle * Time.deltaTime);
			}	else {
				//rigidbody2D.AddTorque(orbitRotateSpeed*angle * Time.deltaTime);
				rigidbody2D.angularVelocity = -travelRotateSpeed*angle * Time.deltaTime;
			}
		}
	
	}

	void Update() {
		switch(state) {
		case MshipState.ORBIT :
			Orbit();
			break;
		case MshipState.TRAVEL :
			Travel ();
			break;
		default:
			Debug.LogError("Invalid mothership state");
			break;
		}
	}
}