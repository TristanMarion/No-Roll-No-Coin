using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorController : MonoBehaviour {
	public Transform player;
	public Transform coin;
	
	void Update () {
		Orbit ();
	}

	void Orbit() {
		Vector3 vector = player.position - coin.position;
		GetComponent<Rigidbody> ().transform.position = player.position - (vector * 0.2f);
	}
}
