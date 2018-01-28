using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	[SerializeField] Character[] players;
	[SerializeField] GameWinPanel winPanel;
	[SerializeField] GameStartPanel startPanel;
	
	void OnValidate() {
		players = GameObject.FindObjectsOfType<Character>();
	}
	
	void Start() {
		stopGame();
		StartCoroutine(startRoutine());
	}
	
	IEnumerator startRoutine() {
		yield return new WaitForSeconds(1);
		yield return startPanel.performRoutine();
		startGame();
	}
	
	public void onWin(bool leftPlayer) {
		StartCoroutine(onWinRoutine(leftPlayer));
	}
	
	IEnumerator onWinRoutine(bool leftPlayer) {
		stopGame();
		yield return new WaitForSeconds(1);
		winPanel.win(leftPlayer ? GameWinPanel.Player.left : GameWinPanel.Player.right);
	}
	
	[ContextMenu("Start game")]
	public void startGame() {
		for (var i = 0; i < players.Length; i++) {
			players[i].enabled = true;
		}
	}
	
	[ContextMenu("Stop game")]
	public void stopGame() {
		for (var i = 0; i < players.Length; i++) {
			players[i].enabled = false;
		}
	}
}
