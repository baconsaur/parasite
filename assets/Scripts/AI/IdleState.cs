using UnityEngine;
using System.Collections;

public class IdleState : IPreyState {
	private readonly PreyCreature prey;

	public IdleState (PreyCreature newPrey) {
		prey = newPrey;
	}

	public void UpdateState() {
		Search ();
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

	void Search () {
		if (prey.target == null || prey.target.gameObject.tag != prey.gameObject.tag) {
			prey.material.color = Color.green;
			GameObject[] others = GameObject.FindGameObjectsWithTag (prey.gameObject.tag);
			int closestIndex = 0;
			for (int i = 0; i < (others.Length - 1); i++) {
				if (Vector3.Distance(prey.gameObject.transform.position, others[i].transform.position) > Vector3.Distance(prey.gameObject.transform.position, others[closestIndex].transform.position)) {
					closestIndex = i;
				}
			}

			prey.target = others [closestIndex].gameObject.transform;
		}
	}

	void Seek () {
		prey.gameObject.transform.LookAt (prey.target.position);
		prey.gameObject.transform.position = Vector3.MoveTowards (prey.gameObject.transform.position, prey.target.position - prey.gameObject.transform.position, prey.moveSpeed * Time.deltaTime);
	}
}