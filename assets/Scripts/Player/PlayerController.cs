using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float moveSpeed;

	[HideInInspector] public int assimilation = 0;
	[HideInInspector] public bool playerDisguised = false;

	void Awake () {
		moveSpeed = 4f;
	}

	void Start () {
	}

	void Update () {
		float verticalTranslation = Input.GetAxis("Vertical") * moveSpeed;
		float horizontalTranslation = Input.GetAxis("Horizontal") * moveSpeed;

		verticalTranslation *= Time.deltaTime;
		horizontalTranslation *= Time.deltaTime;

		transform.Translate(horizontalTranslation, verticalTranslation, 0);

		if (assimilation < 100 && playerDisguised) {
			GetComponent<Renderer> ().material.color = Color.cyan;
			ProximityCheck ();
		} else if (assimilation >= 100) {
			playerDisguised = true;
			TriggerEndGame ();
		}
	}

	void OnTriggerEnter (Collider other) {
		if (!playerDisguised && other.CompareTag ("Prey")) {
			playerDisguised = Random.value * 100 < assimilation;
		}
	}

	void ProximityCheck () {
		GameObject[] preyGroup = GameObject.FindGameObjectsWithTag ("Prey");

		foreach (GameObject prey in preyGroup) {
			PreyCreature preyCreature = prey.GetComponent<PreyCreature> ();
			if (preyCreature.currentState == preyCreature.groupState) {
				if (Vector3.Distance (transform.position, prey.transform.position) < preyCreature.sightRange/4.5) {
					Debug.DrawRay (transform.position, prey.transform.position - transform.position, Color.red, 0.02f, false);
					return;
				}
			}
		}

		playerDisguised = false;
		GetComponent<Renderer> ().material.color = Color.grey;
	}

	public void Assimilate (PreyCreature prey) {
		assimilation += prey.expValue;
		Debug.Log ("Player is " + assimilation + "% assimilated");
	}

	void TriggerEndGame() {
		Debug.Log ("you're fucked bro");
	}
}
