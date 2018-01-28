using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(FadeAnimation))]
[CanEditMultipleObjects]
public class FadeAnimationEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		var obj = (FadeAnimation)target;
		
		GUILayout.Space(10);
		GUILayout.Label("Test editor");
		
		if (GUILayout.Button("Perform")) { obj.perform(); }
	}
}
#endif

public class FadeAnimation : MonoBehaviour {
	[SerializeField] float duration;
	[SerializeField] CanvasGroup canvasGroup;
	
	float? startTime;
	
	public void perform() {
		startTime = Time.time;
		reset();
	}
	
	public void reset() {
		canvasGroup.alpha = 0;
	}
	
	void Update() {
		if (!startTime.HasValue) { return; }
		var time = (Time.time - startTime.Value) / duration;
		
		if (time > 1) {
			canvasGroup.alpha = 1;
			startTime = null;
		} else {
			canvasGroup.alpha = time;
		}
	}
	
	void OnValidate() {
		if (canvasGroup == null) { canvasGroup = GetComponent<CanvasGroup>(); }
	}
}
