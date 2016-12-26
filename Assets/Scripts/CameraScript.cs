using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	GameObject player;
	public float smooth;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 specificVector = new Vector3 (player.transform.position.x + 1, player.transform.position.y + 1.5f, player.transform.position.z - 3.5f);
		transform.position = Vector3.Lerp (transform.position, specificVector, smooth * Time.deltaTime);
	}
}
