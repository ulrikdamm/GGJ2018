using UnityEngine;

public class BatteryDropArea : MonoBehaviour {
	[SerializeField] Robot robot;
	int chargeCount;
	
	public void addCharge() {
		chargeCount += 1;
		
		if (chargeCount >= 3) {
			robot.becomeActive();
			chargeCount = 0;
		} else if (chargeCount == 2) {
			robot.becomeCharged(false);
		} else {
			robot.becomeCharged(true);
		}
	}
}
