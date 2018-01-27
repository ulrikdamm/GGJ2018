using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LaserFlash : MonoBehaviour {
	[SerializeField] Image image;
	
	void Start() {
		image.enabled = false;
	}
	
	public void flash() {
		StartCoroutine(flashRoutine());
	}
	
	IEnumerator flashRoutine() {
		image.enabled = true;
		yield return new WaitForEndOfFrame();
		image.enabled = false;
		yield return new WaitForEndOfFrame();
		image.enabled = true;
		yield return new WaitForEndOfFrame();
		image.enabled = false;
	}
	
	void OnValidate() {
		if (image == null) { image = GetComponent<Image>(); }
	}
}
