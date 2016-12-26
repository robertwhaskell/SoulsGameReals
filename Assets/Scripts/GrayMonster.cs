using UnityEngine;
using System.Collections;

public class GrayMonster : MonoBehaviour {

	public float hp;
	public float speed;
	public float initAttackTimer;
	public float attackTimer;
	public float initRetreatTimer;
	public float retreatTimer;
	public GameObject attackBox;
	float direction;
	bool closingIn = true;
	bool retreating;

	public GameObject player;
	Rigidbody2D rb2d;
	Animator anim;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");

		rb2d = gameObject.GetComponent<Rigidbody2D> ();
		anim = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (player) {
			AttackAndRetreatLoop ();
		}
	}

	//move towards player, attack, fall back, repeat
	void AttackAndRetreatLoop () {
		Vector2 playerPos = player.transform.position;
		if (playerPos.x > transform.position.x) {
			direction = 1;
		} else {
			direction = -1;
		}

		if (closingIn) {
			if (Mathf.Abs (transform.position.x - playerPos.x) < 3.8) {
				closingIn = false;
				attackTimer = initAttackTimer;
				retreatTimer = initRetreatTimer;
				rb2d.velocity = new Vector2 (0, rb2d.velocity.y);
				anim.SetTrigger ("Attack");
				StartCoroutine (CreateAttackBox ());
			} else {
				rb2d.velocity = new Vector2 (speed * direction, rb2d.velocity.y);
			}
		} else if (attackTimer > 0) {
			attackTimer -= Time.deltaTime;
		} else if (attackTimer < 0 && !retreating) {
			retreating = true;
		} else if (retreating && retreatTimer > 0) {
			rb2d.velocity = new Vector2 (speed / 2 * -direction, rb2d.velocity.y);
			retreatTimer -= Time.deltaTime;
		} else if (retreatTimer < 0) {
			retreating = false;
			closingIn = true;
			rb2d.velocity = new Vector2 (0, rb2d.velocity.y);
		} 

		transform.localScale = new Vector3 (-direction, transform.localScale.y, transform.localScale.z);
	}

	IEnumerator CreateAttackBox () {
		yield return new WaitForSeconds (0.1f);
		attackBox.GetComponent<BoxCollider2D> ().enabled = true;
		yield return new WaitForSeconds (0.1f);
		attackBox.GetComponent<BoxCollider2D> ().enabled = false;
	}

	public void TakeDamage (float damage) {
		hp -= damage;
		if (hp <= 0) {
			Die ();
		}
	}

	public void Die () {
		Destroy (gameObject);
	}
}
