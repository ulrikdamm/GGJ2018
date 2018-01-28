using UnityEngine;
using System.Collections;

public class BatteryDropArea : MonoBehaviour {
	[SerializeField] Robot robot;
	[SerializeField] SpriteRenderer spriteRenderer;
	int chargeCount;
	
	[SerializeField] Sprite zeroChargesSprite;
	[SerializeField] Sprite oneChargeSprite;
	[SerializeField] Sprite twoChargesSprite;
	[SerializeField] Sprite threeChargesSprite;
	
	public void addCharge() {
		chargeCount += 1;
		
		if (chargeCount >= 3) {
			robot.becomeActive();
			StartCoroutine(resetChargesRoutine());
		} else if (chargeCount == 2) {
			robot.becomeCharged(false);
		} else {
			robot.becomeCharged(true);
		}
		
		updateChargeCountDisplay();
	}
	
	IEnumerator resetChargesRoutine() {
		yield return new WaitForSeconds(4);
		chargeCount = 0;
		updateChargeCountDisplay();
	}
	
	void updateChargeCountDisplay() {
		switch (chargeCount) {
			case 0: spriteRenderer.sprite = zeroChargesSprite; break;
			case 1: spriteRenderer.sprite = oneChargeSprite; break;
			case 2: spriteRenderer.sprite = twoChargesSprite; break;
			case 3: default: spriteRenderer.sprite = threeChargesSprite; break;
		}
	}
	
	void OnValidate() {
		if (spriteRenderer == null) { spriteRenderer = GetComponent<SpriteRenderer>(); }
	}
}
