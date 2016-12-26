using UnityEngine;
using System.Collections;

public class AttackBox : MonoBehaviour {

	public float damage;
	public Transform source;
	// Use this for initialization
	void Start () {
		gameObject.GetComponent<BoxCollider2D> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	void OnTriggerEnter2D(Collider2D col) {
		Debug.Log (gameObject.name + ", attacking " + col.name + ", enabled == " + gameObject.GetComponent<BoxCollider2D>().enabled);
		if (col.tag == "Body" || col.tag == "Player") {
			RaycastHit2D hit = Physics2D.Raycast(source.transform.position, col.transform.position - source.transform.position, 4, 1 << LayerMask.NameToLayer("Shield"));
			Debug.DrawRay(source.transform.position, col.transform.position - source.transform.position, Color.green);
			//If something was hit, the RaycastHit2D.collider will not be null.
			if (hit.collider != null) {
				Debug.Log ("hit shield");
			} else {
				Debug.Log ("hit body");
				CombatHandler.handler.DealDamage (col.gameObject, damage);
			}
		} 
	}
}