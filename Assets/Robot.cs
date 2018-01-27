using UnityEngine;

public class Robot : MonoBehaviour {
	enum State { sleep, active, charging, attack, veryAttack, damaged, dead };
	State state;
	
	[SerializeField] GameObject shield;
	
	[SerializeField] Sprite[] sleepingSprites;
	[SerializeField] Sprite[] activeSprites;
	[SerializeField] Sprite[] attackingSprites;
	[SerializeField] Sprite[] veryAttackingSprites;
	[SerializeField] Sprite[] deadSprites;
	[SerializeField] Sprite[] deadOnFireSprites;
	[SerializeField] Sprite[] damagedSprites;
	[SerializeField] Sprite[] chargeSprites;
	[SerializeField] Sprite[] chargeDoubleSprites;
	
	[SerializeField] GameObject heart1;
	[SerializeField] GameObject heart2;
	[SerializeField] GameObject heart3;
	
	[SerializeField] AudioClip chargeSound;
	[SerializeField] AudioClip attackSound;
	
	[SerializeField] AudioSource audioSource;
	[SerializeField] SpriteRenderer spriteRenderer;
	[SerializeField] LaserFlash flash;
	
	Sprite[] currentSprites;
	int currentSpriteIndex = 0;
	float spriteAnimationDuration = 0.3f;
	float spriteAnimationCountdown = 0;
	bool loopAnimation = true;
	
	float? fireCountdown = null;
	float? shieldCountdown = null;
	int health = 3;
	
	void Start() {
		becomeSleepy(force: true);
	}
	
	void Update() {
		if (fireCountdown.HasValue) {
			fireCountdown = fireCountdown.Value - Time.deltaTime;
			
			if (fireCountdown.Value < 0) {
				becomeSleepy();
				fireCountdown = null;
				destroyOtherRobot();
			} else if (fireCountdown.Value < 0.5f) {
				becomeVeryAttacky();
			} else if (fireCountdown.Value < 1.5f) {
				becomeAttacky();
			}
		}
		
		spriteAnimationCountdown -= Time.deltaTime;
		if (spriteAnimationCountdown < 0) {
			spriteAnimationCountdown = spriteAnimationDuration;
			currentSpriteIndex = (currentSpriteIndex + 1) % currentSprites.Length;
			if (loopAnimation) { currentSpriteIndex %= currentSprites.Length; }
			else { currentSpriteIndex = Mathf.Min(currentSpriteIndex, currentSprites.Length - 1); }
			spriteRenderer.sprite = currentSprites[currentSpriteIndex];
			
			if (state == State.dead && currentSpriteIndex == deadSprites.Length - 1) {
				showAnimation(deadOnFireSprites, 0.2f);
			}
			
			if (state == State.damaged && currentSpriteIndex == damagedSprites.Length - 1) {
				becomeSleepy();
			}
			
			if (state == State.charging && currentSpriteIndex == chargeSprites.Length - 1) {
				becomeSleepy();
			}
		}
		
		if (shieldCountdown.HasValue) {
			shieldCountdown = shieldCountdown.Value - Time.deltaTime;
			if (shieldCountdown < 0) {
				shieldCountdown = null;
				shield.SetActive(false);
			}
		}
	}
	
	public void becomeSleepy(bool force = false) {
		if (!force && state == State.sleep) { return; }
		state = State.sleep;
		showAnimation(sleepingSprites, 0.5f);
	}
	
	[ContextMenu("Become active")]
	public void becomeActive() {
		if (state == State.active) { return; }
		state = State.active;
		showAnimation(activeSprites, 0.3f);
		fireCountdown = 6;
		audioSource.clip = chargeSound;
		audioSource.Play();
	}
	
	public void becomeCharged(bool doubleCharge) {
		if (state == State.charging) { return; }
		state = State.charging;
		
		if (doubleCharge) {
			showAnimation(chargeDoubleSprites, 0.3f);
		} else {
			showAnimation(chargeSprites, 0.3f);
		}
	}
	
	public void becomeAttacky() {
		if (state == State.attack) { return; }
		state = State.attack;
		showAnimation(attackingSprites, 0.3f);
	}
	
	public void becomeVeryAttacky() {
		if (state == State.veryAttack) { return; }
		state = State.veryAttack;
		showAnimation(veryAttackingSprites, 0.1f);
		audioSource.clip = attackSound;
		audioSource.Play();
	}
	
	public void damage() {
		if (shield.activeSelf) { return; }
		
		health = Mathf.Max(health - 1, 0);
		heart1.SetActive(health >= 1);
		heart2.SetActive(health >= 2);
		heart3.SetActive(health >= 3);
		
		if (health == 0) {
			becomeDead();
		} else {
			becomeDamaged();
		}
	}
	
	public void becomeDead() {
		if (state == State.dead) { return; }
		state = State.dead;
		showAnimation(deadSprites, 0.1f, loop: false);
	}
	
	public void becomeDamaged() {
		if (state == State.damaged) { return; }
		state = State.damaged;
		showAnimation(damagedSprites, 0.3f, loop: false);
	}
	
	void showAnimation(Sprite[] sprites, float interval, bool loop = true) {
		currentSprites = sprites;
		currentSpriteIndex = 0;
		spriteAnimationDuration = interval;
		spriteAnimationCountdown = 0;
		spriteRenderer.sprite = sprites[0];
		loopAnimation = loop;
	}
	
	void destroyOtherRobot() {
		flash.flash();
		
		var others = GameObject.FindObjectsOfType<Robot>();
		for (var i = 0; i < others.Length; i++) {
			var other = others[i];
			if (other == this) { continue; }
			other.damage();
		}
	}
	
	void OnValidate() {
		if (spriteRenderer == null) { spriteRenderer = GetComponent<SpriteRenderer>(); }
		if (audioSource == null) { audioSource = GetComponent<AudioSource>(); }
	}
	
	public void activateShield() {
		shield.SetActive(true);
		shieldCountdown = 4;
	}
}
