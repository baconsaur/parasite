using UnityEngine;
using System.Collections;

public class IdleState : IPreyState {
	private readonly PreyCreature prey;

	public IdleState (PreyCreature newPrey) {
		prey = newPrey;
	}

	public void UpdateState() {
		FindTarget ();
		Seek ();
	}

	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			prey.target = other.gameObject.transform;
			prey.fleeTimer = prey.fleeTime;
			ToFleeState ();
		} else if (other.gameObject.CompareTag ("Prey")) {
			prey.expValue++;
			ToGroupState ();
		}
	}

	public void ToIdleState() {
		Debug.Log ("Can't transition to same state");
	}

	public void ToFleeState() {
		prey.currentState = prey.fleeState;
	}

	public void ToGroupState () {
		prey.currentState = prey.groupState;
	}

	void FindTarget () {
		if (prey.target == null) {
			prey.material.color = Color.green;
			GameObject[] others = GameObject.FindGameObjectsWithTag ("Prey");
			GameObject closest = null;

			foreach (GameObject other in others) {
				PreyCreature otherPreyCreature = other.GetComponent<PreyCreature> ();
				if (otherPreyCreature.currentState == otherPreyCreature.groupState) {
					if (closest == null || Vector3.Distance (prey.transform.position, other.transform.position) < Vector3.Distance (prey.transform.position, closest.transform.position)) {
						closest = other;
					}
				}
			}
				
			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			bool playerDisguised = Random.value * 100 < player.GetComponent<PlayerController> ().assimilation;

			if (playerDisguised && Vector3.Distance (prey.transform.position, player.transform.position) < Vector3.Distance (prey.transform.position, closest.transform.position)) {
				Debug.DrawRay (prey.transform.position, player.transform.position - prey.transform.position, Color.yellow, 1, false);
				prey.target = player.transform;
			} else {
				Debug.DrawRay (prey.transform.position, closest.transform.position - prey.transform.position, Color.green, 1, false);
				prey.target = closest.transform;
			}
		}
	}

	void Seek () {
		prey.gameObject.transform.LookAt (prey.target.position);
		prey.gameObject.transform.position = Vector3.MoveTowards (prey.transform.position, prey.target.position, prey.moveSpeed * Time.deltaTime);
	}
}