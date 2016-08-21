using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float moveSpeed;
	public Mesh trueForm;
	public Mesh dittoForm;

	[HideInInspector] public int assimilation;
	[HideInInspector] public bool playerDisguised;
	[HideInInspector] private Light halo;

	private float baseGlow;

	void Awake () {
		moveSpeed = 4f;
		assimilation = 0;
		playerDisguised = false;
	}

	void Start () {
		halo = GetComponent<Light>();
		baseGlow = halo.range;
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
			float disguiseChance = Random.value * 100;
			if (disguiseChance < assimilation) {
				playerDisguised = true;
			}
		}
	}

	void ProximityCheck () {
		GameObject[] preyGroup = GameObject.FindGameObjectsWithTag ("Prey");

		foreach (GameObject prey in preyGroup) {
			PreyCreature preyCreature = prey.GetComponent<PreyCreature> ();
			if (preyCreature.currentState == preyCreature.groupState) {
				if (Vector3.Distance (transform.position, prey.transform.position) < preyCreature.sightRange) {
					Debug.DrawRay (transform.position, prey.transform.position - transform.position, Color.red, 0.02f, false);
					return;
				}
			}
		}

		if (assimilation < 100) {
			playerDisguised = false;
			GetComponent<MeshFilter> ().mesh = trueForm;
		}
	}

	public void Assimilate (PreyCreature prey) {
		assimilation += prey.expValue;
		halo.range = baseGlow + (assimilation * 0.01f);
		Debug.Log (halo.range);
	}

	public void Assimilate (FoodItem food) {
		assimilation += food.expValue;
		halo.range = baseGlow + (assimilation * 0.01f);
		Debug.Log (halo.range);
	}

	void TriggerEndGame() {
		GetComponent<MeshFilter> ().mesh = dittoForm;
		Debug.Log ("you're fucked bro");
	}
}
