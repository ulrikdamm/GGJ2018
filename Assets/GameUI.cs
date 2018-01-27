using UnityEngine;
using System.Collections.Generic;

public class GameUI : MonoBehaviour {
	[SerializeField] PointsIndicator pointsPlayer1;
	[SerializeField] PointsIndicator pointsPlayer2;
	
	List<EnergyType> player1Points = new List<EnergyType>();
	List<EnergyType> player2Points = new List<EnergyType>();
	
	public void addPlayerPoint(EnergyType type, int playerIndex) {
		if (playerIndex == 0) {
			player1Points.Add(type);
			if (shouldDiscardPoints(player1Points)) { player1Points.Clear(); }
		} else {
			player2Points.Add(type);
			if (shouldDiscardPoints(player2Points)) { player2Points.Clear(); }
		}
		
		updateUI();
	}
	
	bool shouldDiscardPoints(List<EnergyType> points) {
		if (points.Count < 3) { return false; }
		if (points[0] != points[1] || points[0] != points[2]) { return true; }
		return false;
	}
	
	void updateUI() {
		pointsPlayer1.setColors(player1Points.ToArray());
		pointsPlayer2.setColors(player2Points.ToArray());
	}
}
