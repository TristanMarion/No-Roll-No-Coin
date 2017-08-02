using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	float duration = 5.0f;
	float speed = 300.0f;

	public Text countText;
	public Text winText;
	public Text timerText;

	public Vector3 center;
	public Vector3 size;

	public GameObject coin;


	private Rigidbody rb;
	private int count;
	private float timeLeft;
	private Vector3 spawnPosition;
	private bool inGame;
	public VirtualJoystick VirtualJoystick;

	void Start() {
		inGame = false;
		resetUI ();
		rb = GetComponent<Rigidbody> ();
		spawnPosition = rb.transform.position;
		coin.SetActive (false);
		count = 0;
	}

	void Update() {
		if (inGame) {
			timeLeft -= Time.deltaTime;
			SetTimerText ();
			if (timeLeft < 0) {
				timeLeft = 0;
				winText.text = "La partie est finie ! Score: " + count.ToString () + " points";
				GameOver ();
			}
		}
	}

	void FixedUpdate() {
		if (inGame) {
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");

			Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

			rb.AddForce (movement * speed);

			if (VirtualJoystick.InputDirection != Vector3.zero) {
				movement = VirtualJoystick.InputDirection;
				rb.AddForce (movement * speed);
			}
		} else {
			if (Input.GetKeyDown (KeyCode.Return) || Input.touchCount >= 3) {
				StartGame ();
			}
		}
	}

	IEnumerator OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Coin")) {
			other.gameObject.SetActive (false);
			count++;
			SetCountText ();
			timeLeft += 3.0f;
			yield return new WaitForSeconds (1);
			RespawnCoin ();
		}

		if (other.gameObject.CompareTag ("m_deform")) {
			other.gameObject.SetActive (false);
			rb.transform.localScale += new Vector3 (0, 0.6f, 0);
			rb.transform.position += new Vector3 (0, 0.3f, 0);
			yield return new WaitForSeconds (5);
			rb.transform.localScale -= new Vector3 (0, 0.6f, 0);
			rb.transform.position -= new Vector3 (0, 0.3f, 0);
			other.gameObject.SetActive (true);
		} else if (other.gameObject.CompareTag ("b_speed")) {
			other.gameObject.SetActive (false);
			speed *= 1.5f;
			yield return new WaitForSeconds (5);
			speed /= 1.5f;
			other.gameObject.SetActive (true);
		}
	}

	Vector3 GetRandomPos() {
		return center + new Vector3 (Random.Range (-size.x - 1, size.x - 1), 0.5f, Random.Range (-size.z - 1, size.z - 1));
	}

	void RespawnCoin() {
		coin.GetComponent<Transform> ().position = GetRandomPos ();
		coin.SetActive (true);
	}

	void SetCountText() {
		countText.text = "Coins: " + count.ToString ();
	}

	void SetTimerText() {
		timerText.text = "Time remaining: " + timeLeft.ToString ("F1") + "s"; 
	}

	void StartGame() {
		count = 0;
		inGame = true;
		SetTimerText ();
		SetCountText ();
		resetText (winText);
		rb.transform.position = spawnPosition;
		timeLeft = duration;
		coin.GetComponent<Transform> ().position = GetRandomPos ();
		coin.SetActive (true);
		speed = 300;
	}

	void GameOver() {
		inGame = false;
		speed = 0;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		coin.SetActive (false);
	}

	void resetUI() {
		winText.text = "";
		countText.text = "";
		timerText.text = "";
	}

	void resetText(Text text) {
		text.text = "";
	}
}