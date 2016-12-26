using UnityEngine;
using System.Collections;

public class Paladin : MonoBehaviour {

	public float hp;
	Animator anim;
	public NewShield shield;
	public NewAttackBox sword;
	public float attackTimer;

	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			anim.SetTrigger ("Slash");
			sword.attacking = true;
		}
		if (Input.GetMouseButton (1)) {
			anim.SetBool ("Block", true);
			shield.defending = true;
		}
		if (Input.GetMouseButtonUp (1)) {
			anim.SetBool ("Block", false);
			shield.defending = false;
		}
		anim.SetFloat ("Blend", Input.GetAxis ("Horizontal"));
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

	IEnumerator AttackTimer () {
		yield return new WaitForSeconds (attackTimer);
		sword.attacking = false;
	}
}
