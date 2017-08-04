using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusMalusController : MonoBehaviour {
	public string type;
	public PlayerController player;
	private Transform playerRBTransform;

	void Start() {
		playerRBTransform = player.gameObject.GetComponent<Rigidbody> ().transform;
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			StartCoroutine (type);
			print (type);
		}
	}

	void ToggleObject(GameObject obj, bool enabled) {
		obj.GetComponent<Collider>().enabled = enabled;
		obj.GetComponent<Renderer>().enabled = enabled;
	}

	void DeformPlayer(int coeff) {
		playerRBTransform.localScale += (new Vector3 (0, 0.6f, 0) * coeff);
		playerRBTransform.position += (new Vector3 (0, 0.3f, 0) * coeff);
	}

	IEnumerator Deform() {
		print ("Deform");
		ToggleObject (this.gameObject, false);
		DeformPlayer (1);
		yield return new WaitForSeconds (5);
		StopCoroutine (type);
		ToggleObject (this.gameObject, true);
		DeformPlayer (-1);
	}

	IEnumerator Speed() {
		print ("Speed");
		ToggleObject (this.gameObject, false);
		player.speed *= 1.5f; 
		yield return new WaitForSeconds (5);
		StopCoroutine (type);
		ToggleObject (this.gameObject, true);
		player.speed /= 1.5f;
	}
}
