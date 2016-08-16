using UnityEngine;
using System.Collections;

public class FleeState : IPreyState {
	private readonly PreyCreature prey;

	public FleeState (PreyCreature newPrey) {
		prey = newPrey;
	}

	public void UpdateState() {
		Flee ();
	}

	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Prey")) {
			PreyCreature preyCreature = other.GetComponent<PreyCreature> ();
			if (preyCreature.currentState != preyCreature.fleeState && Random.value * 100 < preyCreature.fleeChance) {
				preyCreature.target = prey.target;
				preyCreature.currentState.ToFleeState ();
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
		Debug.Log ("Can't transition to same state");
	}

	public void ToGroupState () {
		prey.target = prey.TargetClosest ("Food");
		prey.currentState = prey.groupState;
	}
		
	private void Flee() {
		prey.material.color = Color.red;
		if (prey.fleeTimer > 0) {
			if (Vector3.Distance (prey.transform.position, prey.target.position) < prey.sightRange) {
				prey.fleeTimer = prey.fleeTime;
				prey.currentSpeed = prey.maxSpeed;
			} else {
				prey.currentSpeed = prey.minSpeed;
				prey.fleeTimer -= Time.deltaTime;
			}
			prey.gameObject.transform.LookAt (-prey.target.position);

			prey.gameObject.transform.position = Vector3.MoveTowards (prey.transform.position, prey.target.position, -prey.currentSpeed * Time.deltaTime);
		} else if (Vector3.Distance(prey.transform.position, prey.TargetClosest("Food").position) < 3) {
			ToGroupState ();
		} else {
			ToIdleState ();
		}
	}
}