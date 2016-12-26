using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

	public float hp;
	public bool facingRight = true;
	public float speed;
	public GameObject attackBox;
	public GameObject defendBox;
	bool defending;
	GameObject body;
	Rigidbody2D rb2d;
	Animator anim;

	void Awake () {
		Transform[] ts = gameObject.GetComponentsInChildren<Transform>();
		foreach (Transform t in ts) {
			if (t.tag == "Body") {
				body = t.gameObject;
				break;
			}
		}
		rb2d = body.GetComponent<Rigidbody2D> ();
		anim = body.GetComponent<Animator> ();
	}

	// Use this for initialization
	void Start () {
		defendBox.GetComponent<BoxCollider2D> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		HandleActions ();
		HandleMovement ();
	}

	void HandleActions () {
		if (Input.GetMouseButtonDown (0)) {
			anim.SetTrigger ("Attack");
			StartCoroutine (CreateAttackBox ());
		} else if (Input.GetMouseButton (1)) {
			anim.SetBool ("Defending", true);
			CreateDefendBox ();
		} else {
			anim.SetBool ("Defending", false);
			DestroyDefendBox ();
		}
	}

	void HandleMovement () {
		var speedModifier = defending ? speed / 4 : speed;
		float direction = Input.GetAxis ("Horizontal");
		if (direction < 0.1f && direction > -0.1f) {
			rb2d.velocity = new Vector2 (0, rb2d.velocity.y);
		} else {
			rb2d.velocity = new Vector2(speedModifier * direction, rb2d.velocity.y);
		}

		if (!defending) {
			if (direction >= 0.1) {
				body.transform.localScale = new Vector3 (1, 1, 1);
				facingRight = true;
			} else if (direction <= -0.1) {
				body.transform.localScale = new Vector3 (-1, 1, 1);
				facingRight = false;
			}
		}
	}

	IEnumerator CreateAttackBox (){
		float attackDirection = facingRight ? 1 : -1;
		yield return new WaitForSeconds (0.1f);
		attackBox.transform.position = new Vector2 (body.transform.position.x + 2.11f * attackDirection, body.transform.position.y + 1.08f);
		attackBox.GetComponent<BoxCollider2D> ().enabled = true;
		yield return new WaitForSeconds (0.1f);
		attackBox.GetComponent<BoxCollider2D> ().enabled = false;
	}

	void CreateDefendBox (){
		float defendDirection = facingRight ? 1 : -1;
		defending = true;
		defendBox.transform.position = new Vector2 (body.transform.position.x + 1.57f * defendDirection, body.transform.position.y);
		defendBox.GetComponent<BoxCollider2D> ().enabled = true;
	}

	void DestroyDefendBox () {
		defending = false;
		defendBox.GetComponent<BoxCollider2D> ().enabled = false;
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
