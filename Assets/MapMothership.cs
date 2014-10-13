using UnityEngine;
using System.Collections;

public enum MshipState {
	ORBIT, TRAVEL
};

public class MapMothership : MonoBehaviour {
	GraphNode gn;
	float orbitDist = 10.0f;
	float orbitTargetRotateSpeed = 0.2f;
	float orbitRotateSpeed = 100.0f;
	float orbitMoveSpeed = 500.0f;
	float travelRotateSpeed = 500.0f;
	float travelMoveSpeed = 4000.0f;
	float travelDist;

	MshipState state;
	Vector2 orbitVec;	

	void Start() {
		orbitVec = new Vector2(1.0f, 0.0f);
		state = MshipState.ORBIT;
	}

	public void SetState(MshipState s, GraphNode n) {
		state = s;
		gn = n;

		if(s == MshipState.TRAVEL)
			travelDist = (gn.transform.position - transform.position).magnitude - orbitDist;

	}

	public bool IsOrbiting() {
		return state == MshipState.ORBIT;
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
	
		Vector2 forward = -transform.up.normalized;
		Vector2 left = transform.right.normalized;
		
		Vector2 moveDir = (targetPos - (Vector2)transform.position);
		Vector2 f = moveDir * orbitMoveSpeed * Time.deltaTime;
		rigidbody2D.velocity = f;

		//rotation		
		float angle = Vector2.Angle(forward, moveDir.normalized);
		float dot = Vector2.Dot (moveDir.normalized, left);

		if(dot > 0.0f) {
			rigidbody2D.angularVelocity = orbitRotateSpeed*angle * Time.deltaTime;
		}	else {
			rigidbody2D.angularVelocity = -orbitRotateSpeed*angle * Time.deltaTime;
		}
	}

	void Travel() {
		Vector2 targetPos = gn.transform.position;

		Vector2 forward = -transform.up.normalized;
		Vector2 left = transform.right.normalized;
		
		Vector2 moveDir = (targetPos - (Vector2)transform.position);
		float magnitude = moveDir.magnitude;
		moveDir = moveDir.normalized;

		if(magnitude < orbitDist) {
			SetState(MshipState.ORBIT, gn);
			orbitVec = -moveDir.normalized;
			return;
		}
	
		float vel;
		float t = Mathf.Max((travelDist-magnitude)/travelDist,0);
		vel = Mathf.Lerp(travelMoveSpeed, 300.0f, t);

		rigidbody2D.velocity = moveDir * vel * Time.deltaTime;

		//rotation
		float angle = Vector2.Angle(forward, moveDir.normalized);
		float dot = Vector2.Dot (moveDir.normalized, left);
		if(dot > 0.0f) {
			rigidbody2D.angularVelocity = travelRotateSpeed*angle * Time.deltaTime;
		}	else {
			rigidbody2D.angularVelocity = -travelRotateSpeed*angle * Time.deltaTime;
		}
	}

	void FixedUpdate() {
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