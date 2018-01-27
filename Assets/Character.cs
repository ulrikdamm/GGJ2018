using UnityEngine;
using System.Collections.Generic;

public class Character : MonoBehaviour {
	[SerializeField] int playerIndex;
	
	[SerializeField] Sprite normalSprite;
	[SerializeField] Sprite carryingSprite;
	[SerializeField] Sprite placingSprite;
	[SerializeField] Sprite punchingSprite;
	
	[SerializeField] Pickup pickupPrefab;
	
	[SerializeField] KeyCode upKey;
	[SerializeField] KeyCode downKey;
	[SerializeField] KeyCode leftKey;
	[SerializeField] KeyCode rightKey;
	[SerializeField] KeyCode actionKey;
	[SerializeField][Range(0, 2)] float speed;
	
	[SerializeField] Rigidbody2D body;
	[SerializeField] new SpriteRenderer renderer;
	[SerializeField] PunchZone punchZone;
	
	[SerializeField] PowerupType slowdownPowerup;
	[SerializeField] PowerupType speedupPowerup;
	
	List<GameObject> overObjects = new List<GameObject>();
	
	EnergyType carrying;
	Vector3 initialScale;
	GameUI gameUI;
	
	public float slowdownTimer = 0;
	public float speedupTimer = 0;
	float? putDownTimer = null;
	float? punchTimer = null;
	public Vector2 direction;
	
	void Start() {
		initialScale = transform.localScale;
		gameUI = GameObject.FindObjectOfType<GameUI>();
	}
	
	void Update() {
		slowdownTimer = Mathf.Max(slowdownTimer - Time.deltaTime, 0);
		speedupTimer = Mathf.Max(speedupTimer - Time.deltaTime, 0);
		
		if (putDownTimer.HasValue) {
			putDownTimer = putDownTimer.Value - Time.deltaTime;
			if (putDownTimer < 0) {
				putDownBattery();
			}
		}
		
		if (punchTimer.HasValue) {
			punchTimer = punchTimer.Value - Time.deltaTime;
			if (punchTimer < 0) {
				renderer.sprite = normalSprite;
				punchTimer = null;
			}
		}
	}
	
	void FixedUpdate() {
		var speed = this.speed;
		if (speedupTimer > 0.01) { speed *= 2; }
		if (slowdownTimer > 0.01f) { speed /= 2; }
		
		var position = body.position;
		
		if (Input.GetKey(upKey)) {
			position += new Vector2(0, 1) * speed;
			direction = new Vector2(0, 1);
		}
		
		if (Input.GetKey(downKey)) {
			position += new Vector2(0, -1) * speed;
			direction = new Vector2(0, -1);
		}
		
		if (Input.GetKey(leftKey)) {
			position += new Vector2(-1, 0) * speed;
			setDirection(left: true);
			direction = new Vector2(-1, 0);
		}
		
		if (Input.GetKey(rightKey)) {
			position += new Vector2(1, 0) * speed;
			setDirection(left: false);
			direction = new Vector2(1, 0);
		}
		
		body.MovePosition(position);
		
		if (Input.GetKeyDown(actionKey)) { pickupItem(); }
		if (Input.GetKeyUp(actionKey) && putDownTimer.HasValue) { cancelPutdown(); }
	}
	
	void pickupItem() {
		if (punchTimer.HasValue) { return; }
		
		var pickup = overPickup();
		
		if (isInBatteryDrop() && carrying != null) {
			beginPutdown();
		} else if (carrying != null) {
			dropBattery();
		} else if (pickup != null) {
			renderer.sprite = carryingSprite;
			carrying = pickup.type;
			Destroy(pickup.gameObject);
		} else {
			punchZone.punch();
			renderer.sprite = punchingSprite;
			punchTimer = 0.3f;
		}
	}
	
	Pickup overPickup() {
		for (var i = 0; i < overObjects.Count; i++) {
			var pickup = overObjects[i].GetComponent<Pickup>();
			if (pickup != null) {
				return pickup;
			}
		}
		
		return null;
	}
	
	public void dropBattery() {
		if (carrying == null) { return; }
		var spawn = GameObject.Instantiate(pickupPrefab, transform.position, Quaternion.identity);
		spawn.setType(carrying);
		carrying = null;
		renderer.sprite = normalSprite;
	}
	
	void beginPutdown() {
		putDownTimer = 2;
		renderer.sprite = placingSprite;
	}
	
	void putDownBattery() {
		gameUI.addPlayerPoint(carrying, playerIndex);
		carrying = null;
		putDownTimer = null;
		renderer.sprite = normalSprite;
	}
	
	public void cancelPutdown() {
		if (carrying == null) { return; }
		putDownTimer = null;
		renderer.sprite = carryingSprite;
	}
	
	bool isInBatteryDrop() {
		for (var i = 0; i < overObjects.Count; i++) {
			if (overObjects[i].GetComponent<BatteryDropArea>()) {
				return true;
			}
		}
		
		return false;
	}
	
	void setDirection(bool left) {
		var scale = initialScale;
		
		if (left) {
			scale.x *= -1;
		} else {
			scale.x *= 1;
		}
		
		transform.localScale = scale;
	}
	
	void OnValidate() {
		if (body == null) { body = GetComponent<Rigidbody2D>(); }
		if (renderer == null) { renderer = GetComponent<SpriteRenderer>(); }
		if (punchZone == null) { punchZone = GetComponentInChildren<PunchZone>(); }
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		overObjects.Add(other.gameObject);
		
		// var pickup = other.gameObject.GetComponent<Pickup>();
		// if (pickup != null) {
		// 	gameUI.addPlayerPoint(pickup.type, playerIndex);
		// 	Destroy(other.gameObject);
		// 	return;
		// }
		
		var powerup = other.gameObject.GetComponent<Powerup>();
		if (powerup != null) {
			if (powerup.type == slowdownPowerup) { applySlowdownPowerup(); }
			if (powerup.type == speedupPowerup) { applySpeedupPowerup(); }
			Destroy(other.gameObject);
			return;
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		overObjects.Remove(other.gameObject);
	}
	
	public void applySlowdownPowerup() {
		var others = GameObject.FindObjectsOfType<Character>();
		for (var i = 0; i < others.Length; i++) {
			var other = others[i];
			if (other == this) { continue; }
			other.speedupTimer = 0;
			other.slowdownTimer = 5;
		}
	}
	
	public void applySpeedupPowerup() {
		speedupTimer = 5;
		slowdownTimer = 0;
	}
	
	public void pushback(Vector2 direction) {
		body.MovePosition(body.position - direction);
	}
}
