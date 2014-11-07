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
			if (Input.GetKeyDown(KeyCode.UpArrow) && index == 1) {
				MoveForward();
			}

			// steering
			if(index == 1)
			{
				float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
				transform.Rotate(0, rotation, 0);
			}
		} else { // ps move
			// forward
			if (moveController.Data.Acceleration.z >= 400) {
				MoveForward();
			}

			// steering
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, moveController.Data.Orientation.z, transform.rotation.eulerAngles.z);
		}
	}

	protected float GetRotationValue(float value) {
		if (value < 0) {
			float result = value % 360;
			return result + 360;
		} else {
			return value % 360f;
		}
	}
}
