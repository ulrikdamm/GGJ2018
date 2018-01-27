using UnityEngine;

public class Powerup : MonoBehaviour {
	[SerializeField] AnimationCurve popCurve;
	[SerializeField] PowerupType[] types;
	public PowerupType type;
	
	[SerializeField] SpriteRenderer spriteRenderer;
	
	float? popTime;
	Vector3 initialScale;
	
	void Start() {
		var typeIndex = Random.Range(0, types.Length);
		type = types[typeIndex];
		spriteRenderer.sprite = type.sprite;
		
		initialScale = transform.localScale;
		popTime = Time.time;
		transform.localScale = Vector3.zero;
		GetComponent<BoxCollider2D>().enabled = false;
	}
	
	void Update() {
		if (popTime.HasValue) {
			var time = (Time.time - popTime.Value) / 0.5f;
			if (time > 1) {
				transform.localScale = initialScale;
				popTime = null;
				GetComponent<BoxCollider2D>().enabled = true;
			} else {
				 transform.localScale = initialScale * popCurve.Evaluate(time);
			}
		}
	}
	
	void OnValidate() {
		if (spriteRenderer == null) { spriteRenderer = GetComponent<SpriteRenderer>(); }
	}
}
