using UnityEngine;
using System.Collections;

public class CharacterBaseController : MonoBehaviour {

	// AI
	public bool isComputer = false;

	// floating algorithm
	protected Vector3 firstPosition;
	protected float floatingSpeed = 6f;
	protected float maxHeight = 0.4f;
	
	protected float rotationSpeed = 3f;
	protected float forwardSpeed = 40f;

	protected virtual void Awake() {
		if (isComputer) {
			gameObject.AddComponent<AIController>();
			Destroy(gameObject.GetComponent<PlayerController>());
		}
	}
	
	// Use this for initialization
	protected virtual void Start () {
		firstPosition = transform.position;
	}

	// Update is called once per frame
	protected virtual void Update () {
		// idle floating
		Floating ();
	}

	protected virtual void FixedUpdate() {

	}
	
	protected void Floating() {
		transform.position = new Vector3 (transform.position.x, firstPosition.y + (((Mathf.Sin (Time.time * floatingSpeed) + 1) / 2f) * maxHeight), transform.position.z);
	}
	
	protected void MoveForward() {
		rigidbody.AddForce (transform.forward * forwardSpeed);
	}
}
