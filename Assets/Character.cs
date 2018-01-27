using UnityEngine;

public class Character : MonoBehaviour {
	[SerializeField] int playerIndex;
	
	[SerializeField] KeyCode upKey;
	[SerializeField] KeyCode downKey;
	[SerializeField] KeyCode leftKey;
	[SerializeField] KeyCode rightKey;
	[SerializeField][Range(0, 2)] float speed;
	
	[SerializeField] Rigidbody2D body;
	[SerializeField] new SpriteRenderer renderer;
	
	[SerializeField] PowerupType slowdownPowerup;
	[SerializeField] PowerupType speedupPowerup;
	
	Vector3 initialScale;
	GameUI gameUI;
	
	public float slowdownTimer = 0;
	public float speedupTimer = 0;
	
	void Start() {
		initialScale = transform.localScale;
		gameUI = GameObject.FindObjectOfType<GameUI>();
	}
	
	void Update() {
		slowdownTimer = Mathf.Max(slowdownTimer - Time.deltaTime, 0);
		speedupTimer = Mathf.Max(speedupTimer - Time.deltaTime, 0);
	}
	
	void FixedUpdate() {
		var position = body.position;
		
		var speed = this.speed;
		if (speedupTimer > 0.01) { speed *= 2; }
		if (slowdownTimer > 0.01f) { speed /= 2; }
		
		if (Input.GetKey(upKey)) { position += new Vector2(0, 1) * speed; }
		if (Input.GetKey(downKey)) { position += new Vector2(0, -1) * speed; }
		
		if (Input.GetKey(leftKey)) {
			position += new Vector2(-1, 0) * speed;
			setDirection(left: true);
		}
		
		if (Input.GetKey(rightKey)) {
			position += new Vector2(1, 0) * speed;
			setDirection(left: false);
		}
		
		body.MovePosition(position);
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
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		var pickup = other.gameObject.GetComponent<Pickup>();
		if (pickup != null) {
			gameUI.addPlayerPoint(pickup.type, playerIndex);
			Destroy(other.gameObject);
			return;
		}
		
		var powerup = other.gameObject.GetComponent<Powerup>();
		if (powerup != null) {
			if (powerup.type == slowdownPowerup) { applySlowdownPowerup(); }
			if (powerup.type == speedupPowerup) { applySpeedupPowerup(); }
			Destroy(other.gameObject);
			return;
		}
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
}
