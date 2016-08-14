using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float moveSpeed;

	[HideInInspector] public int assimilation = 0;

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
	}

	public void Assimilate (PreyCreature prey) {
		assimilation += prey.expValue;
		Debug.Log ("Player is " + assimilation + "% assimilated");
	}
}
