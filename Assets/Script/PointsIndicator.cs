using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(PointsIndicator))]
[CanEditMultipleObjects]
public class PointsIndicatorEditor : Editor {
	EnergyType type1;
	EnergyType type2;
	EnergyType type3;
	
	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		var obj = (PointsIndicator)target;
		
		GUILayout.Space(10);
		GUILayout.Label("Test editor");
		
		type1 = (EnergyType)EditorGUILayout.ObjectField(type1, typeof(EnergyType), allowSceneObjects: false);
		type2 = (EnergyType)EditorGUILayout.ObjectField(type2, typeof(EnergyType), allowSceneObjects: false);
		type3 = (EnergyType)EditorGUILayout.ObjectField(type3, typeof(EnergyType), allowSceneObjects: false);
		
		if (GUILayout.Button("Set energies")) {
			obj.setColors(new EnergyType[] { type1, type2, type3 });
		}
	}
}
#endif

public class PointsIndicator : MonoBehaviour {
	[SerializeField] Image[] points;
	[SerializeField] Color offColor;
	
	void OnValidate() {
		points = GetComponentsInChildren<Image>();
	}
	
	void Start() {
		setColors(new EnergyType[0]);
	}
	
	public void setColors(EnergyType[] types) {
		for (var i = 0; i < points.Length; i++) {
			if (i >= types.Length || types[i] == null) {
				points[i].color = Color.gray;
			} else {
				points[i].color = types[i].color;
			}
		}
	}
}
