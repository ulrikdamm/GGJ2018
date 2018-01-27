using UnityEngine;

public class Robot : MonoBehaviour {
	enum State { sleep, active, attack, veryAttack };
	State state;
	
	[SerializeField] Sprite[] sleepingSprites;
	[SerializeField] Sprite[] activeSprites;
	[SerializeField] Sprite[] attackingSprites;
	[SerializeField] Sprite[] veryAttackingSprites;
	
	[SerializeField] SpriteRenderer spriteRenderer;
	
	Sprite[] currentSprites;
	int currentSpriteIndex = 0;
	float spriteAnimationDuration = 0.3f;
	float spriteAnimationCountdown = 0;
	
	float? fireCountdown = null;
	
	void Start() {
		becomeSleepy(force: true);
	}
	
	void Update() {
		if (fireCountdown.HasValue) {
			fireCountdown = fireCountdown.Value - Time.deltaTime;
			
			if (fireCountdown.Value < 0) {
				becomeSleepy();
				fireCountdown = null;
			} else if (fireCountdown.Value < 2) {
				becomeVeryAttacky();
			} else if (fireCountdown.Value < 3) {
				becomeAttacky();
			}
		}
		
		spriteAnimationCountdown -= Time.deltaTime;
		if (spriteAnimationCountdown < 0) {
			spriteAnimationCountdown = spriteAnimationDuration;
			currentSpriteIndex = (currentSpriteIndex + 1) % currentSprites.Length;
			spriteRenderer.sprite = currentSprites[currentSpriteIndex];
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
	}
	
	public void becomeAttacky() {
		if (state == State.attack) { return; }
		state = State.attack;
		showAnimation(attackingSprites, 0.3f);
	}
	
	public void becomeVeryAttacky() {
		if (state == State.veryAttack) { return; }
		state = State.veryAttack;
		showAnimation(veryAttackingSprites, 0.2f);
	}
	
	void showAnimation(Sprite[] sprites, float interval) {
		currentSprites = sprites;
		currentSpriteIndex = 0;
		spriteAnimationDuration = interval;
		spriteAnimationCountdown = 0;
		spriteRenderer.sprite = sprites[0];
	}
	
	void OnValidate() {
		if (spriteRenderer == null) { spriteRenderer = GetComponent<SpriteRenderer>(); }
	}
}
