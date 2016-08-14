using UnityEngine;
using System.Collections;

public class PreyCreature : MonoBehaviour {
	public float sightRange;
	public float moveSpeed;
	public float fleeTime;
	public int expValue;

	[HideInInspector] public Transform target;
	[HideInInspector] public float fleeTimer;
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
		sightRange = 3f;
		moveSpeed = 3f;
		fleeTime = 0.5f;
	}

	void Start () {
		currentState = idleState;
	}

	void Update () {
		currentState.UpdateState ();
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag ("Player")) {
			currentState.OnTriggerEnter (other);
		} else if (other.CompareTag ("Prey")) {
			PreyCreature prey = other.GetComponent<PreyCreature>();

			if (currentState != fleeState && prey.currentState == prey.fleeState) {
				currentState.OnTriggerEnter (prey.target.GetComponent<Collider> ());
			} else if (currentState == idleState) {
				currentState.OnTriggerEnter (gameObject.GetComponent<Collider> ());
			}
		}
	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.CompareTag("Player")) {
			PlayerController playerController = other.gameObject.GetComponent<PlayerController> ();
			playerController.Assimilate(this);
			Object.Destroy(gameObject);
		}
	}
}