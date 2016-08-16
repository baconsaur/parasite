using UnityEngine;
using System.Collections;

public class GroupState : IPreyState {
	private readonly PreyCreature prey;

	public GroupState (PreyCreature newPrey) {
		prey = newPrey;
	}

	public void UpdateState() {
		Follow ();
	}

	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			if (other.GetComponent<PlayerController>().playerDisguised == false) {
				prey.target = other.gameObject.transform;
				ToFleeState ();
			}
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
		prey.target = prey.TargetClosest ("Food");
		prey.currentState = prey.idleState;
	}

	public void ToFleeState() {
		prey.fleeTimer = prey.fleeTime;
		prey.currentState = prey.fleeState;
	}

	public void ToGroupState () {
		Debug.Log ("Can't transition to same state");
	}

	private void Follow() {
		prey.material.color = Color.yellow;
		if (prey.target == null) {
			ToIdleState ();
		} else {
			prey.transform.LookAt (prey.target.position);
			prey.transform.position = Vector3.MoveTowards (prey.transform.position, prey.target.position, prey.currentSpeed * Time.deltaTime);
		}
	}
}
