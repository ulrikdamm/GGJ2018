using UnityEngine;

public class Pickup : MonoBehaviour {
	[SerializeField] EnergyType[] types;
	public EnergyType type;
	
	[SerializeField] SpriteRenderer spriteRenderer;
	
	void Start() {
		if (type == null) {
			var typeIndex = Random.Range(0, types.Length);
			setType(types[typeIndex]);
		}
	}
	
	public void setType(EnergyType type) {
		this.type = type;
		// spriteRenderer.color = type.color;
	}
	
	void OnValidate() {
		if (spriteRenderer == null) { spriteRenderer = GetComponent<SpriteRenderer>(); }
	}
}
