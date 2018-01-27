using UnityEngine;

public class BatteryDropArea : MonoBehaviour {
	[SerializeField] Robot robot;
	int chargeCount;
	
	public void addCharge() {
		chargeCount += 1;
		
		if (chargeCount >= 3) {
			robot.becomeActive();
		}
	}
}
