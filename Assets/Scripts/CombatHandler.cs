using UnityEngine;
using System.Collections;

public class CombatHandler : MonoBehaviour {

	public static CombatHandler handler;

	void Awake () {
		if (handler == null) {
			handler = this;
		} else if (handler != this) {
			Destroy(gameObject); 
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DealDamage (GameObject recipient, float damage) {
		switch (recipient.transform.tag) {
		case "Player":
			recipient.transform.GetComponent<Paladin> ().TakeDamage (damage);
			break;
		case "Enemy":
			recipient.transform.GetComponent<Skeleton> ().TakeDamage (damage);
			break;
		}
	}
}
