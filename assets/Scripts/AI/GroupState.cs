using UnityEngine;
using System.Collections;

public class GroupState : IPreyState {
	private readonly PreyCreature prey;

	public GroupState (PreyCreature newPrey) {
		prey = newPrey;
	}

	public void UpdateState() {
		Group ();
	}

	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			prey.target = other.gameObject.transform;
			prey.fleeTimer = prey.fleeTime;
			ToFleeState ();
		}
	}

	public void ToIdleState() {
		
	}

	public void ToFleeState() {
		prey.currentState = prey.fleeState;
	}

	public void ToGroupState () {
		Debug.Log ("Can't transition to same state");
	}

	private void Group() {
		prey.material.color = Color.yellow;
	}
}
