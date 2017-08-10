using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusMalusController : MonoBehaviour {
	public string type;
	public PlayerController player;
	public GameObject particleRenderer;
	private Transform playerRBTransform;

	public AudioSource bonusSoundSource;
	public AudioSource malusSoundSource;
	public AudioClip bonusSoundClip;
	public AudioClip malusSoundClip;

	void Start() {
		bonusSoundSource.clip = bonusSoundClip;
		malusSoundSource.clip = malusSoundClip;
		playerRBTransform = player.gameObject.GetComponent<Rigidbody> ().transform;
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			StartCoroutine (type);
			ToggleObject (this.gameObject, false);
			print (type);
		}
	}

	public void ToggleObject(GameObject obj, bool enabled) {
		obj.GetComponent<Collider>().enabled = enabled;
		obj.GetComponent<Renderer>().enabled = enabled;
		if (particleRenderer != null)
			particleRenderer.SetActive (enabled);
	}

	void DeformPlayer(int coeff) {
		playerRBTransform.localScale += (new Vector3 (0, 1.5f, 0) * coeff);
		playerRBTransform.position += (new Vector3 (0, 0.75f, 0) * coeff);
	}

	IEnumerator Deform() {
		malusSoundSource.Play ();
		print ("Deform");
		DeformPlayer (1);
		yield return new WaitForSeconds (5);
		StopCoroutine (type);
		ToggleObject (this.gameObject, true);
		DeformPlayer (-1);
	}

	IEnumerator Speed() {
		bonusSoundSource.Play ();
		print ("Speed");
		player.speed *= 1.5f; 
		yield return new WaitForSeconds (5);
		StopCoroutine (type);
		ToggleObject (this.gameObject, true);
		player.speed /= 1.5f;
	}

	IEnumerator SpeedDown() {
		malusSoundSource.Play ();
		print ("SpeedDown");
		player.speed *= 0.5f; 
		yield return new WaitForSeconds (5);
		StopCoroutine (type);
		ToggleObject (this.gameObject, true);
		player.speed /= 0.5f;
	}

	IEnumerator SizeUp() {
		bonusSoundSource.Play ();
		print ("SizeUp");
		playerRBTransform.localScale *= 2; 
		playerRBTransform.position += new Vector3(0.0f, 1.0f, 0.0f);
		yield return new WaitForSeconds (5);
		StopCoroutine (type);
		ToggleObject (this.gameObject, true);
		playerRBTransform.position -= new Vector3(0.0f, 1.0f, 0.0f);
		playerRBTransform.localScale /= 2;
	}

	IEnumerator SizeDown() {
		malusSoundSource.Play ();
		print ("SizeDown");
		playerRBTransform.localScale /= 2; 
		playerRBTransform.position -= new Vector3(0.0f, 0.25f, 0.0f);
		yield return new WaitForSeconds (5);
		StopCoroutine (type);
		ToggleObject (this.gameObject, true);
		playerRBTransform.position += new Vector3(0.0f, 0.25f, 0.0f);
		playerRBTransform.localScale *= 2; 
	}
}
