using UnityEngine;
using System.Collections.Generic;

public class Character : MonoBehaviour {
	enum State { stand, walk, standCarry, walkCarry, placing, punching };
	State state;
	
	[SerializeField] int playerIndex;
	
	[SerializeField] AudioClip punchSound;
	[SerializeField] AudioClip gettingPunchedSound;
	
	[SerializeField] Sprite normalSprite;
	[SerializeField] Sprite[] walkSprites;
	[SerializeField] Sprite[] carryingSprites;
	[SerializeField] Sprite placingSprite;
	[SerializeField] Sprite punchingSprite;
	
	[SerializeField] Pickup pickupPrefab;
	
	[SerializeField] KeyCode upKey;
	[SerializeField] KeyCode downKey;
	[SerializeField] KeyCode leftKey;
	[SerializeField] KeyCode rightKey;
	[SerializeField] KeyCode actionKey;
	[SerializeField][Range(0, 2)] float speed;
	
	[SerializeField] Robot robot;
	[SerializeField] Rigidbody2D body;
	[SerializeField] new SpriteRenderer renderer;
	[SerializeField] PunchZone punchZone;
	[SerializeField] AudioSource audioSource;
	
	[SerializeField] PowerupType slowdownPowerup;
	[SerializeField] PowerupType speedupPowerup;
	[SerializeField] PowerupType shieldPowerup;
	
	List<GameObject> overObjects = new List<GameObject>();
	
	EnergyType carrying;
	Vector3 initialScale;
	GameUI gameUI;
	
	public float slowdownTimer = 0;
	public float speedupTimer = 0;
	float? putDownTimer = null;
	float? punchTimer = null;
	public Vector2 direction;
	Vector2 pushbackForce;
	
	Sprite[] currentSprites;
	int currentSpriteIndex = 0;
	float spriteAnimationDuration = 0.3f;
	float spriteAnimationCountdown = 0;
	bool loopAnimation = true;
	
	bool moving = false;
	
	void Start() {
		initialScale = transform.localScale;
		gameUI = GameObject.FindObjectOfType<GameUI>();
		setState(State.stand, force: true);
	}
	
	void setState(State state, bool force = false) {
		if (!force && this.state == state) { return; }
		this.state = state;
		
		switch (state) {
			case State.stand: showSprite(walkSprites[0]); break;
			case State.walk: showAnimation(walkSprites, 0.1f); break;
			case State.standCarry: showSprite(carryingSprites[0]); break;
			case State.walkCarry: showAnimation(carryingSprites, 0.1f); break;
			case State.placing: showSprite(placingSprite); break;
			case State.punching: showSprite(punchingSprite); break;
		}
	}
	
	void showSprite(Sprite sprite) {
		showAnimation(new Sprite[] { sprite }, 10);
	}
	
	void showAnimation(Sprite[] sprites, float interval, bool loop = true) {
		currentSprites = sprites;
		currentSpriteIndex = 0;
		spriteAnimationDuration = interval;
		spriteAnimationCountdown = 0;
		renderer.sprite = sprites[0];
		loopAnimation = loop;
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
				setState(State.stand);
				// showSprite(normalSprite);
				// renderer.sprite = normalSprite;
				punchTimer = null;
			}
		}
		
		if (Input.GetKeyDown(actionKey)) { pickupItem(); }
		if (Input.GetKeyUp(actionKey) && putDownTimer.HasValue) { cancelPutdown(); }
		
		spriteAnimationCountdown -= Time.deltaTime;
		if (spriteAnimationCountdown < 0) {
			spriteAnimationCountdown = spriteAnimationDuration;
			currentSpriteIndex = (currentSpriteIndex + 1) % currentSprites.Length;
			if (loopAnimation) { currentSpriteIndex %= currentSprites.Length; }
			else { currentSpriteIndex = Mathf.Min(currentSpriteIndex, currentSprites.Length - 1); }
			renderer.sprite = currentSprites[currentSpriteIndex];
		}
		
		if (state == State.placing) {
			moving = false;
		} else {
			moving = false;
			
			var newDirection = Vector2.zero;
			
			if (Input.GetKey(upKey)) {
				newDirection.y = 1;
				moving = true;
			} else if (Input.GetKey(downKey)) {
				newDirection.y = -1;
				moving = true;
			}
			
			if (Input.GetKey(leftKey)) {
				setDirection(left: true);
				newDirection.x = -1;
				moving = true;
			} else if (Input.GetKey(rightKey)) {
				setDirection(left: false);
				newDirection.x = 1;
				moving = true;
			}
			
			if (moving) {
				direction = newDirection;
			}
			
			if (state != State.punching) {
				if (moving) {
					setState(carrying == null ? State.walk : State.walkCarry);
				} else {
					setState(carrying == null ? State.stand : State.standCarry);
				}
			}
		}
	}
	
	void FixedUpdate() {
		var speed = this.speed;
		if (speedupTimer > 0.01) { speed *= 1.5f; }
		if (slowdownTimer > 0.01f) { speed /= 1.5f; }
		
		var position = body.position;
		
		if (moving) {
			position += direction * speed;
		}
		
		// if (state != State.placing) {
		// 	moving = false;
			
		// 	if (Input.GetKey(upKey)) {
		// 		position += new Vector2(0, 1) * speed;
		// 		direction = new Vector2(0, 1);
		// 		moving = true;
		// 	} else if (Input.GetKey(downKey)) {
		// 		position += new Vector2(0, -1) * speed;
		// 		direction = new Vector2(0, -1);
		// 		moving = true;
		// 	}
			
		// 	if (Input.GetKey(leftKey)) {
		// 		position += new Vector2(-1, 0) * speed;
		// 		setDirection(left: true);
		// 		direction = new Vector2(-1, 0);
		// 		moving = true;
		// 	} else if (Input.GetKey(rightKey)) {
		// 		position += new Vector2(1, 0) * speed;
		// 		setDirection(left: false);
		// 		direction = new Vector2(1, 0);
		// 		moving = true;
		// 	}
			
		// 	if (state != State.punching) {
		// 		if (moving) {
		// 			setState(carrying == null ? State.walk : State.walkCarry);
		// 		} else {
		// 			setState(carrying == null ? State.stand : State.standCarry);
		// 		}
		// 	}
		// }
		
		if (pushbackForce.magnitude > 0.01f) {
			position -= pushbackForce;
			pushbackForce /= 2;
		}
		
		body.MovePosition(position);
	}
	
	void pickupItem() {
		if (punchTimer.HasValue) { return; }
		
		var pickup = overPickup();
		
		if (inBatteryDrop() != null && carrying != null) {
			beginPutdown();
		} else if (carrying != null) {
			dropBattery();
		} else if (pickup != null) {
			// showSprite(carryingSprite);
			// renderer.sprite = carryingSprite;
			carrying = pickup.type;
			setState(State.standCarry);
			Destroy(pickup.gameObject);
		} else {
			punchZone.punch();
			// showSprite(punchingSprite);
			// renderer.sprite = punchingSprite;
			setState(State.punching);
			punchTimer = 0.3f;
			audioSource.clip = punchSound;
			audioSource.Play();
		}
	}
	
	Pickup overPickup() {
		for (var i = 0; i < overObjects.Count; i++) {
			if (overObjects[i] == null) {
				overObjects.RemoveAt(i);
				i -= 1;
				continue;
			}
			
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
		setState(State.stand);
		// showSprite(normalSprite);
		// renderer.sprite = normalSprite;
	}
	
	void beginPutdown() {
		putDownTimer = 1;
		setState(State.placing);
		// showSprite(placingSprite);
		// renderer.sprite = placingSprite;
	}
	
	void putDownBattery() {
		var dropArea = inBatteryDrop();
		if (dropArea == null) { return; }
		
		gameUI.addPlayerPoint(carrying, playerIndex);
		carrying = null;
		putDownTimer = null;
		setState(State.stand);
		// showSprite(normalSprite);
		// renderer.sprite = normalSprite;
		inBatteryDrop().addCharge();
	}
	
	public void cancelPutdown() {
		if (carrying == null) { return; }
		putDownTimer = null;
		setState(State.standCarry);
		// showSprite(carryingSprite);
		// renderer.sprite = carryingSprite;
	}
	
	BatteryDropArea inBatteryDrop() {
		for (var i = 0; i < overObjects.Count; i++) {
			var dropArea = overObjects[i].GetComponent<BatteryDropArea>();
			if (dropArea != null) { return dropArea; }
		}
		
		return null;
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
		if (audioSource == null) { audioSource = GetComponent<AudioSource>(); }
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
			if (powerup.type == shieldPowerup) { robot.activateShield(); }
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
		audioSource.clip = gettingPunchedSound;
		audioSource.Play();
		pushbackForce = direction * 2;
	}
}
