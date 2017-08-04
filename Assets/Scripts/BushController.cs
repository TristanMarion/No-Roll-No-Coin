using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushController : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			other.gameObject.GetComponent<Rigidbody> ().drag *= 10;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			other.gameObject.GetComponent<Rigidbody> ().drag /= 10;
		}
	}
}
