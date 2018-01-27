using UnityEngine;

public class Powerup : MonoBehaviour {
	[SerializeField] PowerupType[] types;
	public PowerupType type;
	
	[SerializeField] SpriteRenderer spriteRenderer;
	
	void Start() {
		var typeIndex = Random.Range(0, types.Length);
		type = types[typeIndex];
		spriteRenderer.sprite = type.sprite;
	}
	
	void OnValidate() {
		if (spriteRenderer == null) { spriteRenderer = GetComponent<SpriteRenderer>(); }
	}
}
