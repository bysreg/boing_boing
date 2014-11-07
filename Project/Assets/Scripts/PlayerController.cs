using UnityEngine;
using System.Collections;

public class PlayerController : CharacterBaseController {

	bool simulateWithKeyboard = false;

	//ps move
	MoveController moveController;
	Vector3 initialOrientation;
	public int psMoveIndex = 0;

	// Use this for initialization
	protected override void Start () {
		base.Start ();

		try {
			if(PSMoveInput.IsConnected && PSMoveInput.MoveControllers[psMoveIndex].Connected) {
				simulateWithKeyboard = false;
			} else {
				simulateWithKeyboard = true;
			}
		} catch {
			simulateWithKeyboard = true;
		}

		if (!simulateWithKeyboard) {
			moveController = PSMoveInput.MoveControllers[psMoveIndex];
		}
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();

		// keboard
		if (simulateWithKeyboard) {
			// forward
			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				MoveForward();
			}
			// steering
			float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
			transform.Rotate(0, rotation, 0);
		} else { // ps move
			// forward
			if (Mathf.Abs(moveController.Data.Acceleration.y) >= 100) {
				MoveForward();
			}

			// steering
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, moveController.Data.Orientation.y, transform.rotation.eulerAngles.z);
		}
	}
}
