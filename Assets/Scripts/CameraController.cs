using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject player;

	private Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
/*	void LateUpdate () {
		transform.position = player.transform.position + offset;
	}*/

	public VirtualJoystick cameraJoystick;
	public Transform lookAt;

	private float distance = 30.0f;
	private float currentX = 90.0f;
	//private float currentY = 0.0f;
	//private float sensivityX = 10.0f;
	//private float sensivityY = 10.0f;

	private void Update()
	{
		currentX += cameraJoystick.InputDirection.x;
		//currentY += cameraJoystick.InputDirection.z;
	}

	private void LateUpdate()
	{
		transform.position = player.transform.position + offset;
		Vector3 dir = new Vector3(0, player.transform.position.y, -distance);
		Quaternion rotation = Quaternion.Euler(0, -currentX, 0);
		transform.position = lookAt.position + rotation * dir;
		transform.LookAt (lookAt);
	}
}
