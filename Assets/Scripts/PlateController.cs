using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateController : MonoBehaviour {
	public string color;
	public Material material;
	public PlayerController playerController;
	private string[] colors = {"red", "green"};

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag ("Player")) {
			ChangeObstaclesColor (color);
		}
	}

	public void ChangeObstaclesColor(string wantedColor) {
		for (int i = 0; i < colors.Length; i++) {
			bool isGoodColor = colors [i] == wantedColor;
			GameObject[] objects = GameObject.FindGameObjectsWithTag (colors[i] + "_obstacle");
			for (int j = 0; j < objects.Length; j++) {
				GameObject obj = objects [j];
				Color color = obj.GetComponent<Renderer> ().material.color;
				obj.GetComponent<Collider> ().enabled = !isGoodColor;
				obj.GetComponent<Renderer> ().material.color = new Color(color.r, color.g, color.b, isGoodColor? 0f : 1.0f);
			}
		}
		playerController.changeColor (material);
	}
}
