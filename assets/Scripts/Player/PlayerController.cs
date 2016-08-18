using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float moveSpeed;
	public Mesh trueForm;
	public Mesh dittoForm;

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
			GetComponent<MeshFilter> ().mesh = dittoForm;
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
		GetComponent<MeshFilter> ().mesh = trueForm;
	}

	public void Assimilate (PreyCreature prey) {
		assimilation += prey.expValue;
		Debug.Log ("Player is " + assimilation + "% assimilated");
	}

	public void Assimilate (FoodItem food) {
		assimilation += food.expValue;
		Debug.Log ("Player is " + assimilation + "% assimilated");
	}

	void TriggerEndGame() {
		Debug.Log ("you're fucked bro");
	}
}
