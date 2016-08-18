using UnityEngine;
using System.Collections;

public class IdleState : IPreyState {
	private readonly PreyCreature prey;

	public IdleState (PreyCreature newPrey) {
		prey = newPrey;
	}

	public void UpdateState() {
		Seek ();
	}

	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Player") && other.GetComponent<PlayerController>().playerDisguised == false) {
			prey.target = other.gameObject.transform;
			ToFleeState ();
		} else if (other.gameObject.CompareTag ("Food")) {
			ToGroupState ();
		}
	}

	public void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.CompareTag("Player")) {
			PlayerController playerController = collision.gameObject.GetComponent<PlayerController> ();
			if (playerController.assimilation < 100) {
				playerController.Assimilate (prey);
				Object.Destroy (prey.gameObject);
			}
		}
	}

	public void ToIdleState() {
		Debug.Log ("Can't transition to same state");
	}

	public void ToFleeState() {
		prey.fleeTimer = prey.fleeTime;
		prey.currentState = prey.fleeState;
	}

	public void ToGroupState () {
		prey.currentState = prey.groupState;
	}

	void Seek () {
//		prey.material.color = Color.green;
		if (prey.target == null) {
			prey.target = prey.TargetClosest ("Food");
		}
		prey.gameObject.transform.LookAt (prey.target.position);
		prey.gameObject.transform.position = Vector3.MoveTowards (prey.transform.position, prey.target.position, prey.currentSpeed * Time.deltaTime);
	}
}