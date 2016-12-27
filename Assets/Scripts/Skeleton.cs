using UnityEngine;
using System.Collections;

public class Skeleton : MonoBehaviour {

	public bool increaseSpeed;

	public float hp;
	Animator anim;
	Rigidbody rb2d;
	float direction = -1;
	GameObject player;
	public NewAttackBox sword;
	public bool dead;
	public bool closingIn;
	public bool retreating;
	public float initAttackTimer;
	public float initRetreatTimer;
	public float attackTimer;
	public float retreatTimer;
	public float speed;
	public float blend;
	public float blendSmooth;
	public float walkSpeed;
	public float runSpeed;
	public bool running;
	public float attackDistance;
	public float runDistance;

	void Awake() {
		anim = gameObject.GetComponent<Animator> ();
		rb2d = gameObject.GetComponent<Rigidbody> ();
	}

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		closingIn = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (dead) {
			rb2d.velocity = new Vector3 (0, 0, 0);
			rb2d.isKinematic = true;
			gameObject.GetComponent<CapsuleCollider> ().enabled = false;
			this.enabled = false;
		} else {
			AdjustSpeed ();
			AttackAndRetreatLoop ();
		}

	}

	void AdjustSpeed () {
		var deltaTime = Time.deltaTime / blendSmooth;
		if (running) {
			if (speed <= runSpeed) {
				speed += deltaTime;
				blend += deltaTime / 2;
			} else {
				speed = runSpeed;
				blend = 1;
			}
		} else if (closingIn) {
			if (speed <= walkSpeed) {
				speed += deltaTime;
				blend += deltaTime / 2;
			} else {
				speed = walkSpeed;
				blend = 0.5f;
			}
		} else if (retreating) {
			if (speed >= -walkSpeed) {
				speed -= deltaTime;
				blend -= deltaTime / 2;
			} else {
				speed = -walkSpeed;
				blend = -0.5f;
			}
		}
	}

	void AttackAndRetreatLoop () {
		Vector2 playerPos = player.transform.position;
		if (playerPos.x > transform.position.x) {
			direction = 1;
		} else {
			direction = -1;
		}

		if (closingIn) {
			if (Mathf.Abs (transform.position.x - playerPos.x) < attackDistance){
				closingIn = false;
				running = false;
				attackTimer = initAttackTimer;
				retreatTimer = initRetreatTimer;
				rb2d.velocity = new Vector3 (0, rb2d.velocity.y);
				anim.SetTrigger ("Attack");
				blend = 0;
				speed = 0;
				anim.SetFloat ("Blend", blend);
				sword.attacking = true;
			} else if (Mathf.Abs (transform.position.x - playerPos.x) < runDistance) {
				anim.SetFloat ("Blend", blend);
				running = true;
				//speed = runSpeed;
				rb2d.velocity = new Vector3 (speed * direction, rb2d.velocity.y);
			} else {
				anim.SetFloat ("Blend", blend);
				//speed = walkSpeed;
				rb2d.velocity = new Vector3 (speed * direction, rb2d.velocity.y);
			}
		} else if (attackTimer > 0) {
			attackTimer -= Time.deltaTime;
		} else if (attackTimer < 0 && !retreating) {
			sword.attacking = false;
			retreating = true;
		} else if (retreating && retreatTimer > 0) {
			anim.SetFloat ("Blend", blend);
			//speed = walkSpeed;
			rb2d.velocity = new Vector3 (speed * direction, rb2d.velocity.y);
			retreatTimer -= Time.deltaTime;
		} else if (retreatTimer < 0) {
			retreating = false;
			closingIn = true;
		} 
	}

	public void TakeDamage (float damage) {
		hp -= damage;
		if (hp <= 0) {
			Die ();
		} 
		anim.SetTrigger ("Take Hit");
	}

	public void Die () {
		//Destroy (gameObject);
		dead = true;
		anim.SetBool ("Dead", dead);
	}
}
