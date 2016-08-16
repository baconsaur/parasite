using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	public GameObject spawnPrefab;
	public float spawnChance;
	public float maxCooldown;
	public float spawnRadius;
	public int initialSpawnCount;

	private float cooldown;

	void Awake () {
		for (int i = 0; i < initialSpawnCount; i++) {
			Spawn ();
		}
	}

	public virtual void Update () {
		if (cooldown > 0) {
			cooldown -= Time.deltaTime;
		} else {
			InvokeRepeating ("checkSpawn", 1, 1);
		}
	}

	private void checkSpawn () {
		if (Random.value * 100 < spawnChance) {
			CancelInvoke();
			Spawn ();
			cooldown = maxCooldown;
		}
	}

	private void Spawn () {
		Vector3 spawnerSpawn = new Vector3 (
			Random.Range (transform.position.x - spawnRadius, transform.position.x + spawnRadius), 
			Random.Range (transform.position.y - spawnRadius, transform.position.y + spawnRadius),
			0);
		Instantiate (spawnPrefab, spawnerSpawn, new Quaternion());
	}
}
