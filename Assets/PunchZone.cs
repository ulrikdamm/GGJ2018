using UnityEngine;
using System.Collections.Generic;

public class PunchZone : MonoBehaviour {
	List<Character> charactersInside = new List<Character>();
	Character goodGuy;
	
	void Start() {
		goodGuy = transform.parent.GetComponent<Character>();
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		var character = other.gameObject.GetComponent<Character>();
		if (character != null) { charactersInside.Add(character); }
	}
	
	void OnTriggerExit2D(Collider2D other) {
		var character = other.gameObject.GetComponent<Character>();
		if (character != null) { charactersInside.Remove(character); }
	}
	
	public void punch() {
		for (var i = 0; i < charactersInside.Count; i++) {
			var character = charactersInside[i];
			if (character.gameObject == transform.parent.gameObject) { continue; }
			
			character.cancelPutdown();
			character.dropBattery();
			character.pushback(direction: goodGuy.direction * -1);
		}
	}
}
