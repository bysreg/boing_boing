using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// floating algorithm
	protected Vector3 firstPosition;
	protected float floatingSpeed = 6f;
	protected float maxHeight = 0.4f;

	protected float rotationSpeed = 3f;
	protected float forwardSpeed = 2500f;

	// Use this for initialization
	void Start () {
		firstPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		// idle floating
		Floating ();

		// steering
		float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
		transform.Rotate(0, rotation, 0);
	}

	void FixedUpdate() {
		// acceleration
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			MoveForward();
		}
	}

	void Floating() {
		transform.position = new Vector3 (transform.position.x, firstPosition.y + (((Mathf.Sin (Time.time * floatingSpeed) + 1) / 2f) * maxHeight), transform.position.z);
	}

	void MoveForward() {
		rigidbody.AddForce (transform.forward * forwardSpeed);
	}
}
