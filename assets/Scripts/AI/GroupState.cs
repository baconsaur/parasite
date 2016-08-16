using UnityEngine;
using System.Collections;

public class GroupState : IPreyState {
	private readonly PreyCreature prey;

	public GroupState (PreyCreature newPrey) {
		prey = newPrey;
	}

	public void UpdateState() {
		Watch ();
	}

	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
//			if (other.GetComponent<PlayerController>().playerDisguised == false) {
				prey.target = other.gameObject.transform;
				ToFleeState ();
//			}
		}
	}

	public void OnCollisionEnter(Collision collision) {
		
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

	private void Watch() {
		prey.material.color = Color.yellow;
		if (prey.target == null) {
			ToIdleState ();
		}
	}
}
