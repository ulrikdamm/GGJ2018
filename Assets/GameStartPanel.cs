using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(GameStartPanel))]
[CanEditMultipleObjects]
public class GameStartPanelEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		var obj = (GameStartPanel)target;
		
		GUILayout.Space(10);
		GUILayout.Label("Test editor");
		
		if (GUILayout.Button("Perform")) { obj.perform(); }
	}
}
#endif

public class GameStartPanel : MonoBehaviour {
	[SerializeField] NumberSlideAnimation number1;
	[SerializeField] NumberSlideAnimation number2;
	[SerializeField] NumberSlideAnimation number3;
	[SerializeField] NumberSlideAnimation numberGo;
	[SerializeField] SlideAnimation slide;
	[SerializeField] FadeAnimation fade;
	
	public void perform() {
		StartCoroutine(performRoutine());
	}
	
	public IEnumerator performRoutine() {
		slide.gameObject.SetActive(true);
		fade.gameObject.SetActive(true);
		number1.gameObject.SetActive(false);
		number2.gameObject.SetActive(false);
		number3.gameObject.SetActive(false);
		numberGo.gameObject.SetActive(false);
		fade.perform();
		slide.perform(slideIn: true);
		number3.perform();
		yield return new WaitForSeconds(1);
		number2.perform();
		yield return new WaitForSeconds(1);
		number1.perform();
		yield return new WaitForSeconds(1);
		numberGo.perform();
		yield return new WaitForSeconds(1.5f);
		slide.perform(slideIn: false);
		fade.gameObject.SetActive(false);
	}
}
