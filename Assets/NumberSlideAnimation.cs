using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(NumberSlideAnimation))]
[CanEditMultipleObjects]
public class NumberSlideAnimationEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		var obj = (NumberSlideAnimation)target;
		
		GUILayout.Space(10);
		GUILayout.Label("Test editor");
		
		if (GUILayout.Button("Perform")) { obj.perform(); }
	}
}
#endif

public class NumberSlideAnimation : MonoBehaviour {
	[SerializeField] AnimationCurve positionCurve;
	[SerializeField] AnimationCurve alphaCurve;
	[SerializeField] float duration = 1;
	[SerializeField] float positionMul = 10;
	[SerializeField] RectTransform rectTransform;
	[SerializeField] CanvasGroup canvasGroup;
	
	float? startTime;
	
	public void perform() {
		reset();
		startTime = Time.time;
		gameObject.SetActive(true);
	}
	
	public void reset() {
		canvasGroup.alpha = 0;
	}
	
	void Update() {
		if (!startTime.HasValue) { return; }
		var time = (Time.time - startTime.Value) / duration;
		
		if (time > 1) {
			startTime = null;
			gameObject.SetActive(false);
		} else {
			var pos = rectTransform.anchoredPosition;
			pos.x = positionCurve.Evaluate(time) * positionMul - positionMul / 2;
			rectTransform.anchoredPosition = pos;
			
			canvasGroup.alpha = alphaCurve.Evaluate(time);
		}
	}
	
	void OnValidate() {
		if (rectTransform == null) { rectTransform = GetComponent<RectTransform>(); }
		if (canvasGroup == null) { canvasGroup = GetComponent<CanvasGroup>(); }
	}
}
