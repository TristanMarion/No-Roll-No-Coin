using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	private float SPEED_VALUE = 300.0f;
	private float INITIAL_DURATION = 15.0f;

	public float speed;
	public Text countText;
	public Text winText;
	public Text timerText;
	public Vector3 center;
	public Vector3 size;
	public GameObject coin;
	public Material defaultMaterial;
	public PlateController plateController;

	private Rigidbody rb;
	private int count;
	private float timeLeft;
	private Vector3 spawnPosition;
	private bool inGame;
	public VirtualJoystick VirtualJoystick;

	void Start() {
		speed = SPEED_VALUE;
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

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Coin")) {
			other.gameObject.SetActive (false);
			count++;
			SetCountText ();
			timeLeft += 3.0f;
			RespawnCoin ();
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
		timeLeft = INITIAL_DURATION;
		coin.GetComponent<Transform> ().position = GetRandomPos ();
		coin.SetActive (true);
		speed = SPEED_VALUE;
		plateController.ChangeObstaclesColor ("white");
		changeColor (defaultMaterial);
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

	public void changeColor(Material material) {
		GetComponent<Renderer>().material = material;
	}
}