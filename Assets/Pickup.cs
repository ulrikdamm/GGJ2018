using UnityEngine;

public class Pickup : MonoBehaviour {
	[SerializeField] EnergyType[] types;
	public EnergyType type;
	
	[SerializeField] SpriteRenderer spriteRenderer;
	
	void Start() {
		var typeIndex = Random.Range(0, types.Length);
		type = types[typeIndex];
		spriteRenderer.color = type.color;
	}
	
	void OnValidate() {
		if (spriteRenderer == null) { spriteRenderer = GetComponent<SpriteRenderer>(); }
	}
}
