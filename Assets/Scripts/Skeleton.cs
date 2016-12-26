using UnityEngine;
using System.Collections;

public class Skeleton : MonoBehaviour {

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
	public float walkSpeed;
	public float runSpeed;
	public float runModifier = 1;
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
			gameObject.GetComponent<BoxCollider> ().enabled = false;
			this.enabled = false;
		} else {
			AttackAndRetreatLoop ();
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
				attackTimer = initAttackTimer;
				retreatTimer = initRetreatTimer;
				rb2d.velocity = new Vector3 (0, rb2d.velocity.y);
				anim.SetTrigger ("Attack");
				anim.SetFloat ("Blend", 0);
				sword.attacking = true;
			} else if (Mathf.Abs (transform.position.x - playerPos.x) < runDistance) {
				anim.SetFloat ("Blend", 1);
				speed = runSpeed;
				rb2d.velocity = new Vector3 (speed * direction, rb2d.velocity.y);
			} else {
				anim.SetFloat ("Blend", 0.5f);
				speed = walkSpeed;
				rb2d.velocity = new Vector3 (speed * direction, rb2d.velocity.y);
			}
		} else if (attackTimer > 0) {
			attackTimer -= Time.deltaTime;
		} else if (attackTimer < 0 && !retreating) {
			sword.attacking = false;
			retreating = true;
		} else if (retreating && retreatTimer > 0) {
			anim.SetFloat ("Blend", -0.5f);
			speed = walkSpeed;
			rb2d.velocity = new Vector3 (speed * -direction, rb2d.velocity.y);
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
