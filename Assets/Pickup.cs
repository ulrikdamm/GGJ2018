using UnityEngine;

public class Pickup : MonoBehaviour {
	[SerializeField] AnimationCurve popCurve;
	[SerializeField] EnergyType[] types;
	public EnergyType type;
	
	[SerializeField] SpriteRenderer spriteRenderer;
	
	float? popTime;
	Vector3 initialScale;
	
	void Start() {
		if (type == null) {
			var typeIndex = Random.Range(0, types.Length);
			setType(types[typeIndex]);
		}
		
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
	
	public void setType(EnergyType type) {
		this.type = type;
	}
	
	void OnValidate() {
		if (spriteRenderer == null) { spriteRenderer = GetComponent<SpriteRenderer>(); }
	}
}
