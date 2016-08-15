using UnityEngine;
using System.Collections;

public class FleeState : IPreyState {
	private readonly PreyCreature prey;

	public FleeState (PreyCreature newPrey) {
		prey = newPrey;
	}

	public void UpdateState() {
		Look ();
		Flee ();
	}

	public void OnTriggerEnter(Collider other) {

	}

	public void ToIdleState() {
		prey.currentState = prey.idleState;
		prey.target = null;
	}
		
	public void ToFleeState() {
		Debug.Log ("Can't transition to same state");
	}

	public void ToGroupState () {

	}

	private void Look() {
		RaycastHit hit;

		if (Physics.Raycast (prey.gameObject.transform.position, (prey.target.position - prey.gameObject.transform.position), out hit, prey.sightRange) && hit.collider.CompareTag ("Player")) {
		} else {
			prey.fleeTimer -= Time.deltaTime;
			if (prey.fleeTimer <= 0) {
				ToIdleState ();
			}
		}
	}

	private void Flee() {
		prey.material.color = Color.red;
		prey.gameObject.transform.LookAt (-prey.target.position);
		prey.gameObject.transform.position = Vector3.MoveTowards (prey.gameObject.transform.position, prey.target.position, -prey.moveSpeed * Time.deltaTime);
	}
}