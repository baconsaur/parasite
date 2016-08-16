using UnityEngine;
using System.Collections;

public interface IPreyState {

	void UpdateState();

	void OnTriggerEnter(Collider other);

	void OnCollisionEnter(Collision collision);

	void ToIdleState();

	void ToFleeState();

	void ToGroupState ();

}