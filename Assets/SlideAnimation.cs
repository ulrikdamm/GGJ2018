using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(SlideAnimation))]
[CanEditMultipleObjects]
public class SlideAnimationEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		var obj = (SlideAnimation)target;
		
		GUILayout.Space(10);
		GUILayout.Label("Test editor");
		
		if (GUILayout.Button("Perform in")) { obj.perform(slideIn: true); }
		if (GUILayout.Button("Perform out")) { obj.perform(slideIn: false); }
	}
}
#endif

public class SlideAnimation : MonoBehaviour {
	[SerializeField] float duration;
	[SerializeField] RectTransform rectTransform;
	
	bool slideOut = false;
	
	float? startTime;
	
	public void perform(bool slideIn) {
		slideOut = !slideIn;
		startTime = Time.time;
		reset();
	}
	
	public void reset() {
		var size = rectTransform.sizeDelta;
		size.x = 0;
		rectTransform.sizeDelta = size;
	}
	
	void Update() {
		if (!startTime.HasValue) { return; }
		var time = (Time.time - startTime.Value) / duration;
		
		var size = rectTransform.sizeDelta;
		
		if (time > 1) {
			size.x = (slideOut ? 0 : 1920);
			startTime = null;
		} else {
			size.x = (slideOut ? 1920 - 1920 * time : 1920 * time);
		}
		
		rectTransform.sizeDelta = size;
	}
	
	void OnValidate() {
		if (rectTransform == null) { rectTransform = GetComponent<RectTransform>(); }
	}
}
