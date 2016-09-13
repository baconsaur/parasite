using UnityEngine;
using System.Collections;

public class PreyCreature : MonoBehaviour, IConsumable {
	public float sightRange;
	public float minSpeed;
	public float maxSpeed;
	public float fleeTime;
	public float fleeChance;
	public int expValue;

	[HideInInspector] public Transform target;
	[HideInInspector] public float fleeTimer;
	[HideInInspector] public float currentSpeed;
	[HideInInspector] public IPreyState currentState;
	[HideInInspector] public FleeState fleeState;
	[HideInInspector] public IdleState idleState;
	[HideInInspector] public GroupState groupState;
	[HideInInspector] public Material material;

	private void Awake() {
		fleeState = new FleeState (this);
		idleState = new IdleState (this);
		groupState = new GroupState (this);
		material = GetComponent<Renderer> ().material;
	}

	void Start () {
		target = TargetClosest("Food");
		currentSpeed = minSpeed;
		currentState = idleState;
	}

	void Update () {
		currentState.UpdateState ();
	}

	void OnTriggerEnter(Collider other) {
		currentState.OnTriggerEnter (other);
	}

	void OnCollisionEnter(Collision collision) {
		currentState.OnCollisionEnter (collision);
	}

	public int GetExpValue () {
		return expValue;
	}

	public void Die () {
		Destroy (gameObject);
	}

	public Transform TargetClosest(string type) {
		GameObject[] others = GameObject.FindGameObjectsWithTag (type);
		GameObject closest = null;

		foreach (GameObject other in others) {
			if (closest == null || Vector3.Distance (transform.position, other.transform.position) < Vector3.Distance (transform.position, closest.transform.position)) {
				closest = other;
			}
		}
		if (closest != null) {
			return closest.transform;
		} else {
			int randomTargetIndex = Random.Range (0, others.Length);
			return others [randomTargetIndex].transform;
		}
	}
}