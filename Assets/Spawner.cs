using UnityEngine;

public class Spawner : MonoBehaviour {
	[SerializeField] float minX;
	[SerializeField] float minY;
	[SerializeField] float maxX;
	[SerializeField] float maxY;
	[SerializeField] GameObject[] spawned;
	
	[SerializeField] float baseSpawnChance = 4;
	float spawnChance;
	
	public void spawn() {
		var index = Random.Range(0, spawned.Length);
		var obj = GameObject.Instantiate(spawned[index]);
		
		obj.transform.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
	}
	
	void Update() {
		spawnChance += Time.deltaTime;
		if (Random.Range(0, baseSpawnChance) < spawnChance) {
			spawnChance = 0;
			spawn();
		}
	}
}
