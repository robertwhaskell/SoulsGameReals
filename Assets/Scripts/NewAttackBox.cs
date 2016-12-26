using UnityEngine;
using System.Collections;

public class NewAttackBox : MonoBehaviour {

	public float damage;
	public string blackList;
	public bool attacking { get; set; }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col) {
		if (attacking) {
			if (col.tag == blackList) {
				Debug.Log ("Hit " + col.tag);
				attacking = false;
				CombatHandler.handler.DealDamage (col.gameObject, damage);
			} else if (col.tag == "Defend") {
				if (col.GetComponent<NewShield> ().defending) {
					Debug.Log ("Hit Shield");
					attacking = false;
				}
			}
		}
	}
}
