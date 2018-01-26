using UnityEngine;

public class Spawner : MonoBehaviour {
	[SerializeField] float minX;
	[SerializeField] float minY;
	[SerializeField] float maxX;
	[SerializeField] float maxY;
	[SerializeField] GameObject[] spawned;
	
	[SerializeField][Range(0, 1)] float spawnChance;
	
	public void spawn() {
		var index = Random.Range(0, spawned.Length);
		var obj = GameObject.Instantiate(spawned[index]);
		
		obj.transform.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
	}
	
	void Update() {
		if (Random.Range(0f, 60) < spawnChance) {
			spawn();
		}
	}
}
