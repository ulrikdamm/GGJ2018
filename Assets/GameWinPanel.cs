using UnityEngine;
using UnityEngine.UI;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(GameWinPanel))]
[CanEditMultipleObjects]
public class GameWinPanelEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		var obj = (GameWinPanel)target;
		
		GUILayout.Space(10);
		GUILayout.Label("Test editor");
		
		if (GUILayout.Button("Win left player")) { obj.win(GameWinPanel.Player.left); }
		if (GUILayout.Button("Win right player")) { obj.win(GameWinPanel.Player.right); }
		if (GUILayout.Button("Reset")) { obj.reset(); }
	}
}
#endif

public class GameWinPanel : MonoBehaviour {
	[SerializeField] Color player1WinColor;
	[SerializeField] Color player2WinColor;
	
	[SerializeField] SlideAnimation slide;
	[SerializeField] FadeAnimation fade;
	
	[SerializeField] GameObject player1Label;
	[SerializeField] GameObject player2Label;
	[SerializeField] GameObject menuButton;
	[SerializeField] GameObject replayButton;
	
	public enum Player { left, right };
	
	void Start() {
		reset();
	}
	
	public void win(Player player) {
		slide.gameObject.SetActive(true);
		fade.gameObject.SetActive(true);
		menuButton.SetActive(false);
		replayButton.SetActive(false);
		
		switch (player) {
			case Player.left:
				player2Label.SetActive(false);
				player1Label.SetActive(true);
				fade.GetComponent<Image>().color = player1WinColor;
				break;
			case Player.right:
				player1Label.SetActive(false);
				player2Label.SetActive(true);
				fade.GetComponent<Image>().color = player2WinColor;
				break;
		}
		
		fade.reset();
		slide.reset();
		
		StartCoroutine(winRoutine());
	}
	
	IEnumerator winRoutine() {
		yield return new WaitForSeconds(0.5f);
		fade.perform();
		yield return new WaitForSeconds(0.2f);
		slide.perform(slideIn: true);
		
		yield return new WaitForSeconds(1);
		menuButton.SetActive(true);
		yield return new WaitForSeconds(0.3f);
		replayButton.SetActive(true);
	}
	
	public void reset() {
		slide.gameObject.SetActive(false);
		fade.gameObject.SetActive(false);
		menuButton.SetActive(false);
		replayButton.SetActive(false);
	}
}
