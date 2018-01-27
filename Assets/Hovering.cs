using UnityEngine;

public class Hovering : MonoBehaviour {
	Vector3 initialScale;
	[SerializeField][Range(0, 10)] float amount;
	
	void Awake() {
		initialScale = transform.localScale;
	}
	
	void Update() {
		transform.localScale = initialScale + Vector3.one * amount * Mathf.Sin(Time.time * 2);
	}
}
