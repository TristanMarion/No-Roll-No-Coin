using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	private float SPEED_VALUE = 50.0f;
	private float INITIAL_DURATION = 30.0f;
	private float COIN_GAIN_DURATION = 5.0f;

	public float speed;
	public Text countText;
	public Text winText;
	public Text timerText;
	public Vector3 center;
	public Vector3 size;
	public GameObject coin;
	public Material defaultMaterial;
	public PlateController plateController;
	public VirtualJoystick VirtualJoystick;

	private Rigidbody rb;
	private int count;
	private float timeLeft;
	private Vector3 spawnPosition;
	private bool inGame;
	private Vector3[] coinsPosArr = new [] { 
		new Vector3(20f,21.5f,240f), new Vector3(80f,21.5f,230f), new Vector3(115f,25.5f,250f), new Vector3(100f,25.5f,240f), new Vector3(130f,25.5f,260f), 
		new Vector3(104f,25.5f,220f), new Vector3(91f,25.5f,210f), new Vector3(140f,25.5f,235f), new Vector3(145f,25.5f,245f), new Vector3(130f,25.5f,215f), 
		new Vector3(160f,25.5f,220f), new Vector3(130f,25.5f,190f), new Vector3(150f,25.5f,170f), new Vector3(185f,24.6f,215f),	new Vector3(170f,25.5f,185f),
		new Vector3(185f,25.5f,155f), new Vector3(185f,25.5f,127f), new Vector3(154f,25f,118f), new Vector3(37f,25.5f,205f), new Vector3(85f,25.5f,163f), 
		new Vector3(60f,25.5f,150f), new Vector3(30f,25.5f,158f), new Vector3(9f,25.5f,125f), new Vector3(10f,25.5f,145f), new Vector3(37f,25.5f,171f), 
		new Vector3(88f,25.5f,118f), new Vector3(117f,25.5f,97f), new Vector3(90f,25.5f,90f),new Vector3(65f,25.5f,67f), new Vector3(19f,25.5f,91f),
		new Vector3(90f,25.5f,52f), new Vector3(102f,25.5f,22f), new Vector3(118f,25.5f,21f), new Vector3(134f,25.5f,10f), new Vector3(130f,25.5f,60f),
		new Vector3(177f,25.5f,73f), new Vector3(161f,25.5f,47f), new Vector3(175f,25.5f,17f), new Vector3(191f,25.5f,14f), new Vector3(49f,38.6f,21f), 
		new Vector3(37f,38f,46f), new Vector3(23f,50f,16f), new Vector3(22f,30f,60f), new Vector3(110f,25.5f,140f), new Vector3(130f,21.5f,145f), 
		new Vector3(155f,21.5f,118f), new Vector3(185f,21.5f,105f), new Vector3(65f,25.5f,280f), new Vector3(190f,21.5f,285f), new Vector3(175f,22.5f,255f), 
		new Vector3(165f,22.5f,285f), new Vector3(100f,25.5f,285f)};
	private List<GameObject> bonuses;
	
	void Start() {
		speed = SPEED_VALUE;
		inGame = false;
		resetUI ();
		rb = GetComponent<Rigidbody> ();
		spawnPosition = rb.transform.position;
		coin.SetActive (false);
		count = 0;
		BonusInitStuff ();
	}

	void BonusInitStuff() {
		bonuses = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Bonus"));
		bonuses.AddRange (GameObject.FindGameObjectsWithTag ("Malus"));
	}

	void Update() {
		if (inGame) {
			timeLeft -= Time.deltaTime;
			SetTimerText ();
			if (timeLeft < 0) {
				GameOver ();
			}
		}
	}

	void FixedUpdate() {
		if (inGame) {
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");

			Vector3 movement = new Vector3 (-moveVertical, 0.0f, moveHorizontal);

			rb.AddForce (movement * speed);

			if (VirtualJoystick.InputDirection != Vector3.zero) {
				Vector3 tmp = VirtualJoystick.InputDirection;
				movement = new Vector3 (-tmp.z, 0, tmp.x);
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
			timeLeft += COIN_GAIN_DURATION;
			RespawnCoin ();
		} 
		if (other.gameObject.CompareTag ("spike")) {
			GameOver ();
		}
	}

	Vector3 GetRandomPos() {
		return coinsPosArr[Random.Range (0, coinsPosArr.Length)];
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
		ToggleBonuses (true);
		print (bonuses.Count);
		ResetPlayer ();
	}

	void GameOver() {
		print (bonuses.Count);
		inGame = false;
		speed = 0;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		coin.SetActive (false);
		timeLeft = 0;
		ToggleBonuses (false);
		winText.text = "La partie est terminée ! Score: " + count.ToString () + " points";
	}

	void ToggleBonuses(bool status) {
		for (int i = 0; i < bonuses.Count; i++) {
			bonuses [i].SetActive (status);
			bonuses [i].GetComponent<BonusMalusController> ().ToggleObject (bonuses[i], status);
		}
	}

	void ResetPlayer() {
		rb.transform.localScale = new Vector3 (1.75f, 1.75f, 1.75f);
		speed = SPEED_VALUE;
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