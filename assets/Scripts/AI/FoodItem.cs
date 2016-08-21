using UnityEngine;
using System.Collections;

public class FoodItem : MonoBehaviour {
	public float moveSpeed;
	public int expValue;

	void Start () {
		Vector3 direction = new Vector3 (
			Random.Range(transform.position.x + 1, transform.position.x - 1),
			Random.Range(transform.position.y + 1, transform.position.y - 1),
			0);
		transform.LookAt(direction);
	}

	void Update () {
		transform.position += transform.forward * Time.deltaTime * moveSpeed;
		transform.Rotate(Time.deltaTime, 0, 0);
	}

	void OnCollisionEnter (Collision collision) {
		if (collision.gameObject.CompareTag("Player")) {
			PlayerController playerController = collision.gameObject.GetComponent<PlayerController> ();
			if (playerController.assimilation < 100) {
				playerController.Assimilate (this);
				Object.Destroy (gameObject);
			}
		}
	}
}