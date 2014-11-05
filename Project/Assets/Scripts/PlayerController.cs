using UnityEngine;
using System.Collections;

public class PlayerController : CharacterBaseController {

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();

		// steering
		float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
		transform.Rotate(0, rotation, 0);
	}

	protected override void FixedUpdate() {
		base.FixedUpdate ();

		// acceleration
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			MoveForward();
		}
	}
}
