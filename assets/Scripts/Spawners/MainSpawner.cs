using UnityEngine;
using System.Collections;

public class MainSpawner : Spawner {
	public float cleanUpDistance;
	public int maxSpawners;

	public override void Update () {
		base.Update ();
		CleanUp ();
	}

	private void CleanUp () {
		GameObject[] spawners = GameObject.FindGameObjectsWithTag ("Spawner");
		if (spawners.Length < maxSpawners) {
			foreach (GameObject spawner in spawners) {
				if (Vector3.Distance (transform.position, spawner.transform.position) > cleanUpDistance) {
					Destroy (spawner);
				}
			}
		}
	}
}
