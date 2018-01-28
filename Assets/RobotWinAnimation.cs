using UnityEngine;
using UnityEngine.UI;

public class RobotWinAnimation : MonoBehaviour {
	[SerializeField] Sprite[] sprites;
	[SerializeField] float delay = 0.5f;
	[SerializeField] Image image;
	
	int spriteIndex = 0;
	float countdown = 0;
	
	void Update() {
		countdown -= Time.deltaTime;
		
		if (countdown < 0) {
			spriteIndex = (spriteIndex + 1) % sprites.Length;
			image.sprite = sprites[spriteIndex];
			countdown = delay;
		}
	}
	
	void OnValidate() {
		if (image == null) { image = GetComponent<Image>(); }
	}
}
